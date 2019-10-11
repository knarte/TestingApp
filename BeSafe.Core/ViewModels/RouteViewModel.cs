using BeSafe.Core.Models.Provider;
using BeSafe.Core.Models.DB;
using BeSafe.Core.Models.Internal.Requests;
using BeSafe.Core.Models.Internal.Responses;
using MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using BeSafe.Core.Interfaces;
using MvvmCross.Commands;
using System.Windows.Input;
using System.Threading.Tasks;
using MvvmCross.Plugin.Messenger;
using BeSafe.Core.Services;
using MvvmCross.Plugin.Location;
using BeSafe.Core.Helpers;
using MvvmCross.Plugin.Color;
using System.Drawing;
using System.Globalization;
using MvvmCross.Platform.UI;

namespace BeSafe.Core.ViewModels
{
    public class RouteViewModel: MvxViewModel
    {
        #region Services
        //private DataService dataService;
        private readonly IApiService apiService;
        private readonly IDialogService dialogService;
        private readonly IMvxMessenger messenger;
        private readonly IMvxNativeColor nativeColor;
        private readonly IMvxLocationWatcher watcher;
        private readonly IControlForegroundService controlForegroundService;
        #endregion

        #region Attributes

        private string actualSpeed;
        private string actualSpeedTextColor;
        private string actualSpeedTitle;
        private string speedLimit;
        private string speedLimitTextColor;
        private string speedLimitTitle;

        private bool isVisibleActualSpeedTitle;
        private bool isVisibleActualSpeed;
        private bool isVisibleSpeedLimitTitle;
        private bool isVisibleSpeedLimit;
        private bool isVisibleEndWorkHours;
        private bool isVisibleStartWorkHours;


        private string backgroundColor;
        private int gPSAccuracyIndex;
        private int counter = 0;


        private string GeohashMobile;
        public List<Zone> lstZones;
        public List<ZoneType> lstZonesType = new List<ZoneType>();

        private List<Parameter> lstParameter;
        private List<Event> lstEvent = new List<Event>();
        private List<GPSUserData> lstGPSUserData = new List<GPSUserData>();
        private List<GPSUserData> lstGPSUserDataTemporal = new List<GPSUserData>();
        private List<GPSEvaluated> lstGPSEvaluated = new List<GPSEvaluated>();

        private Location lastLocation = new Location();
        private Location actualLocation = new Location();

        private GPSUserData gpsUserData;
        private int speedLimitZone = 0;
        //private int count;
        private double distanceAccumulation = 0;
        private List<double> distanceList = new List<double>();

        //private bool nearZoneAdvice = true;
        private bool zoneEntryAdvice = true;
        private bool reduceSpeedAdvice = false;
        private bool activateNearSpeedLimitAdvice = true;
        private bool isSpeedExcess = true;
        private bool in_OutZone = false;
        private string lastSpeedLimit = "";
        //private bool gpsState = true;

        private MvxSubscriptionToken token;

        private bool activeMainTimer = true;


        private bool activeSavingGPSDataToDB = true;

        private bool isSuccessLoadZones = false;
        private bool isSuccessLoadParameters = false;

        private double? firstSpeedEvent = 0.0;
        private double? lastSpeedEvent = 0.0;
        private bool adviceGPSMessage = true;
        private bool failLoadParameterFromDB = false;
        private bool failLoadZoneFromDB = false;

        private Event startEventModel = new Event();

        private double testSpeed = 0.0;
        private short testCount = 1;
        #endregion

        #region Properties

        public string ActualSpeed
        {
            get => this.actualSpeed;
            set => this.SetProperty(ref this.actualSpeed, value);
        }

        public string ActualSpeedTextColor
        {
            get => this.actualSpeedTextColor;
            set => this.SetProperty(ref this.actualSpeedTextColor, value);
        }

        public string ActualSpeedTitle
        {
            get => this.actualSpeedTitle;
            set => this.SetProperty(ref this.actualSpeedTitle, value);
        }

        public string SpeedLimit
        {
            get => this.speedLimit;
            set => this.SetProperty(ref this.speedLimit, value);
        }

        public string SpeedLimitTextColor
        {
            get => this.speedLimitTextColor;
            set => this.SetProperty(ref this.speedLimitTextColor, value);
        }

        public string SpeedLimitTitle
        {
            get => this.speedLimitTitle;
            set => this.SetProperty(ref this.speedLimitTitle, value);
        }

        public string BackgroundColor
        {
            get => this.backgroundColor;
            set => this.SetProperty(ref this.backgroundColor, value);
        }

        private int minimunSpeedLimitZone;

        public int GPSAccuracyIndex
        {
            get => this.gPSAccuracyIndex;
            set => this.SetProperty(ref this.gPSAccuracyIndex, value);
        }

        public bool IsVisibleActualSpeedTitle
        {
            get => this.isVisibleActualSpeedTitle;
            set => this.SetProperty(ref this.isVisibleActualSpeedTitle, value);
        }

        public bool IsVisibleActualSpeed
        {
            get => this.isVisibleActualSpeed;
            set => this.SetProperty(ref this.isVisibleActualSpeed, value);
        }

        public bool IsVisibleSpeedLimitTitle
        {
            get => this.isVisibleSpeedLimitTitle;
            set => this.SetProperty(ref this.isVisibleSpeedLimitTitle, value);
        }


        public bool IsVisibleSpeedLimit
        {
            get => this.isVisibleSpeedLimit;
            set => this.SetProperty(ref this.isVisibleSpeedLimit, value);
        }

        public bool IsVisibleEndWorkHours
        {
            get => this.isVisibleEndWorkHours;
            set => this.SetProperty(ref this.isVisibleEndWorkHours, value);
        }

        public bool IsVisibleStartWorkHours
        {
            get => this.isVisibleStartWorkHours;
            set => this.SetProperty(ref this.isVisibleStartWorkHours, value);
        }



        public string varSelectVehicle;
        #endregion

        #region Commands
        private MvxCommand buttonCommand;
        public ICommand ButtonCommand
        {
            get
            {
                this.buttonCommand = this.buttonCommand ?? new MvxCommand(this.ChangeSpeed);
                return this.buttonCommand;
            }
        }

        private MvxCommand startWorkHoursCommand;
        public ICommand StartWorkHoursCommand
        {
            get
            {
                this.startWorkHoursCommand = this.startWorkHoursCommand ?? new MvxCommand(this.StartWorkHours);
                return this.startWorkHoursCommand;
            }
        }

        private MvxCommand endWorkHoursCommand;
        public ICommand EndWorkHoursCommand
        {
            get
            {
                this.endWorkHoursCommand = this.endWorkHoursCommand ?? new MvxCommand(this.EndWorkHours);
                return this.endWorkHoursCommand;
            }
        }


        #endregion

        #region Constructors

        public RouteViewModel(  IApiService apiService,
                                IDialogService dialogService,
                                ILocationService locationService,
                                IMvxMessenger messenger,
                                IControlForegroundService controlForegroundService)
        {
            this.apiService = apiService;
            this.dialogService = dialogService;
            this.messenger = messenger;
            //this.dataService = new DataService();
            this.controlForegroundService = controlForegroundService;
            this.controlForegroundService.CreateService(Settings.SavedInstanceState);
            InitialSettings();
            //ExistDataPendingToSendTextShowing();
            

        }
        #endregion

        #region Testing Methods
        private void ChangeSpeed()
        {
            switch (this.testCount)
            {
                case 1:
                    this.testSpeed = 0;
                    this.testCount++;

                    break;
                case 2:
                    this.testSpeed = 0;
                    this.testCount++;

                    break;
                case 3:
                    this.testSpeed = 10;
                    this.testCount++;

                    break;
                case 4:
                    this.testSpeed = 20;
                    this.testCount++;
                    break;
                case 5:
                    this.testSpeed = 5;
                    this.testCount = 1;
                    break;
                default:
                    this.testSpeed = 0;
                    break;
            }
        }
        #endregion
  
        private void OnLocationMessage(LocationMessage locationMessage)
        {

            var actualLocation = new Location
            {
                Latitude = locationMessage.Lat,
                Longitude = locationMessage.Lng,
                Speed = locationMessage.Speed,
                Accuracy = locationMessage.Accuracy
            };

            UpdateActualSpeed(actualLocation).Wait();


        }

        private async Task UpdateActualSpeed(Location location)
        {
            try
            {
                if (location != null)
                {
                    //location.Speed = this.testSpeed; //This line if to simulate a speed
                    var actualSpeedKM_H = Convert.ToInt32(location.Speed * 3.6);
                    int lastSpeedKM_H = 0;
                    if (this.lastLocation != null)
                    {
                        lastSpeedKM_H = Convert.ToInt32(this.lastLocation.Speed * 3.6);
                    }

                    if (!IsDetectedIrregularSpeedZeros(actualSpeedKM_H, lastSpeedKM_H, location))
                    {
                        this.ActualSpeed = Convert.ToString(actualSpeedKM_H) + "  KM/H";
                        this.actualLocation = location;
                        await CategorizingActualSpeed(minimunSpeedLimitZone);
                    }
                    else
                    {
                        this.ActualSpeed = Convert.ToString(lastSpeedKM_H) + "  KM/H";
                        await CategorizingActualSpeed(minimunSpeedLimitZone);
                    }
                }
            }

            catch (Exception ex)
            {
                var methodName = "UpdateActualSpeed";
                var date = DateTime.UtcNow.ToString();
            }

        }

        private bool IsDetectedIrregularSpeedZeros(int actualSpeed, int lastSpeed, Location location)
        {
            var minimunSpeedAllowsZeros = 7;
            var result = false;
            if (lastSpeed > minimunSpeedAllowsZeros && actualSpeed == 0 && location.Accuracy > 5)
            {
                result = true;
            }
            return result;
        }

        private async void StartWorkHours()
        {
            //ActiveThreads(true);
            this.IsVisibleSpeedLimit = true;
            this.IsVisibleActualSpeed = true;
            this.IsVisibleEndWorkHours = true;
            this.IsVisibleSpeedLimitTitle = true;
            this.IsVisibleActualSpeedTitle = true;

            this.IsVisibleStartWorkHours = false;
            Settings.SavedInstanceState = this.controlForegroundService.StartServices();
            this.token = this.messenger.Subscribe<LocationMessage>(OnLocationMessage);
            //await MainMethod();


        }

        private async void EndWorkHours()
        {
            this.IsVisibleSpeedLimit = false;
            this.IsVisibleActualSpeed = false;
            this.IsVisibleEndWorkHours = true;
            this.IsVisibleSpeedLimitTitle = true;
            this.IsVisibleActualSpeedTitle = true;
            this.IsVisibleStartWorkHours = true;
            Settings.SavedInstanceState = this.controlForegroundService.StopServices();
            this.messenger.Unsubscribe<LocationMessage>(token);
        }

        private void InitialSettings()
        {
            this.IsVisibleSpeedLimit = false;
            this.IsVisibleActualSpeed = false;
            this.IsVisibleEndWorkHours = false;
            this.IsVisibleSpeedLimitTitle = false;
            this.IsVisibleActualSpeedTitle = false;

            this.IsVisibleStartWorkHours = true;

            this.SpeedLimitTitle = "LÍMITE DE VELOCIDAD";
            this.ActualSpeedTitle = "VELOCIDAD ACTUAL";



            this.SpeedLimitTextColor = "white";
            this.ActualSpeedTextColor = "white";
            this.BackgroundColor = "black";

            this.minimunSpeedLimitZone = 60;
            this.SpeedLimit = minimunSpeedLimitZone.ToString() + " KM/H";

            //LoadingText();

        }

        //public async Task CheckDeviceInsideZone(int count)
        //{
        //    try
        //    {

        //        if (count % Endpoint.EXCECUTE_CHECK_DEVICE_INSIDE_ZONE == 0 && counter > 0 && this.adviceGPSMessage)
        //        {
        //            Zone nearestZone = new Zone();
        //            Zone insideZone = new Zone();

        //            var location = this.actualLocation;
        //            if (location != null && location.Latitude != 0 && location.Longitude != 0)
        //            {
        //                //var filteredZones = FilteringZonesMobileIsInside(location);

        //                var filteredZones = this.lstZones;

        //                GetInsideZoneMobile(nearestZone, filteredZones, location);

        //                #region Pendiente por revisar
        //                //var distanceBetweenRadiusAndMobile = GetInsideZoneMobile(nearestZone, filteredZones, location);
        //                //GetNearestZoneToMobile(nearestZone, distanceBetweenRadiusAndMobile)
        //                #endregion

        //                await GetMinimunSpeedLimitZoneIntersection(filteredZones);

        //                //this.lastLocation = this.actualLocation;
        //                this.ActualSpeedTitle = Languages.ActualSpeedTitle;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "CheckDeviceInsideZone";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }
        //}

        //private void GetInsideZoneMobile(Zone nearestZone, List<Zone> filteredZones, Location location)
        //{
        //    try
        //    {
        //        double shortestDistance = 100000000000.0;
        //        double distanceBetweenRadiusAndMobile = 0.0;

        //        foreach (var zone in filteredZones)
        //        {
        //            var locationZone = new Location(zone.CentroidLatitude.Value, zone.CentroidLongitude.Value);
        //            var distanceBetweenZoneAndMobile = location.CalculateDistance(locationZone, DistanceUnits.Kilometers);
        //            distanceBetweenRadiusAndMobile = (distanceBetweenZoneAndMobile * 1000) - zone.Radius;

        //            if (distanceBetweenRadiusAndMobile < 0 && distanceBetweenZoneAndMobile < shortestDistance)
        //            {
        //                shortestDistance = distanceBetweenZoneAndMobile;
        //                nearestZone = zone;

        //                if (distanceBetweenRadiusAndMobile > 0)
        //                {
        //                    this.lstZones.Where(q => q.Id == nearestZone.Id).FirstOrDefault().IsInsideOfZone = false;
        //                }
        //                else
        //                {
        //                    this.lstZones.Where(q => q.Id == nearestZone.Id).FirstOrDefault().IsInsideOfZone = true;
        //                }
        //            }
        //            else
        //            {
        //                this.lstZones.Where(q => q.Id == zone.Id).FirstOrDefault().IsInsideOfZone = false;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "GetInsideZoneMobile";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }
        //}


        //private async Task GetMinimunSpeedLimitZoneIntersection(List<Zone> filteredZones)
        //{
        //    try
        //    {
        //        var minimunSpeedLimitZone = 10000000;
        //        var insideOfActualZones = filteredZones.Where(q => q.IsInsideOfZone == true).ToList();
        //        if (insideOfActualZones.Any())
        //        {

        //            minimunSpeedLimitZone = await GetMinimunSpeedLimit(insideOfActualZones, minimunSpeedLimitZone);

        //            await CategorizingActualSpeed(minimunSpeedLimitZone);

        //            this.SpeedLimitTitle = Languages.SpeedLimitTitle;
        //            this.SpeedLimit = minimunSpeedLimitZone.ToString() + " KM/H";

        //            //SpeedLimitVoiceNotification(minimunSpeedLimitZone, true);
        //        }
        //        else
        //        {
        //            //var listParameter = this.lstParameter.Where(q => q.Parameters == "VelMaximaRural").FirstOrDefault();//await dataService.GetAllParameter("VelMaximaRural");
        //            //minimunSpeedLimitZone = Convert.ToInt32(listParameter.Value.ToString());

        //            await CategorizingActualSpeed(minimunSpeedLimitZone);

        //            this.SpeedLimitTitle = Languages.SpeedLimitTitle;
        //            this.SpeedLimit = minimunSpeedLimitZone.ToString() + " KM/H";
        //            this.speedLimitZone = minimunSpeedLimitZone;
        //            //SpeedLimitVoiceNotification(minimunSpeedLimitZone, false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "GetMinimunSpeedLimitZoneIntersection";
        //        var date = DateTime.UtcNow.ToString();
        //        throw ex;
        //    }

        //}

        private async Task CategorizingActualSpeed(int extractedNameInsideZone)
        {
            try
            {
                var actualSpeedExtracted = 0;

                if (!string.IsNullOrEmpty(this.ActualSpeed))
                {
                    actualSpeedExtracted = Convert.ToInt32(this.ActualSpeed.Substring(0, 3));
                }

                if (actualSpeedExtracted < (extractedNameInsideZone - extractedNameInsideZone * 0.15))
                {
                    ScreenColorSettings("Black", "White", "White");
                    //await SendEventToAPIProcess();
                    this.activateNearSpeedLimitAdvice = true;
                }
                else if (actualSpeedExtracted >= (extractedNameInsideZone - extractedNameInsideZone * 0.15) && actualSpeedExtracted <= extractedNameInsideZone)
                {
                    ScreenColorSettings("Yellow", "Black", "Black");
                    //NearToSpeedLimitVoiceNotification();
                    //await SendEventToAPIProcess();
                }
                else if (actualSpeedExtracted > extractedNameInsideZone)
                {
                    ScreenColorSettings("Red", "White", "White");
                    //GetEventMaxSpeed();
                    //await SpeedExcessVoiceNotification();
                    this.activateNearSpeedLimitAdvice = true;
                }
                else
                {
                    ScreenColorSettings("Black", "White", "White");
                    //await SendEventToAPIProcess();
                    this.activateNearSpeedLimitAdvice = true;
                }
            }
            catch (Exception ex)
            {
                var methodName = "CategorizingActualSpeed";
                var date = DateTime.UtcNow.ToString();
                throw ex;
            }

        }

        private void ScreenColorSettings(string backgroundColor, string speedLimitTextColor, string actualSpeedTextColor)
        {
            try
            {
                this.BackgroundColor = backgroundColor;
                this.SpeedLimitTextColor = speedLimitTextColor;
                this.ActualSpeedTextColor = actualSpeedTextColor;
            }
            catch (Exception ex)
            {
                var methodName = "ScreenColorSettings";
                var date = DateTime.UtcNow.ToString();
                throw ex;
            }

        }

        //private async Task MainMethodProcess()
        //{
        //    try
        //    {

        //        LossGPSSignalMessage();

        //        if (this.adviceGPSMessage)
        //        {

        //            await UpdateActualSpeed();
        //            await CheckDeviceInsideZone(counter);
        //            await SaveGPSUserDataToDB(counter);
        //            await LocateStops(counter);

        //            counter++;
        //            if (counter > 1000)
        //            {
        //                counter = 0;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await MainMethodProcess();
        //    }
        //}



        //private async Task MainMethod()
        //{
        //    try
        //    {
        //        isSuccessLoadZones = await LoadZones();

        //        isSuccessLoadParameters = await LoadParameters();

        //        CheckIfLoadDataFromAPISuccesfully();

        //        await ExistDataPendingToSend();

        //        if (isSuccessLoadZones && isSuccessLoadParameters)
        //        {
        //            await ReadGPS();
        //        }
        //        else
        //        {
        //            await MainMethod();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "MainMethod";
        //        await MainMethod();
        //        ActiveThreads(false);
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //private async Task ReadGPS()
        //{
        //    request = new GeolocationRequest(GeolocationAccuracy.Best);

        //    var isActivatedMainTimer = this.lstParameter.Where(q => q.Parameters == Endpoint.MAIN_TIMER);

        //    var isActivated = isActivatedMainTimer.Count() == 0 ? false : true;

        //    var mainTimer = Endpoint.DEFAULT_MAIN_TIMER;

        //    if (isActivated)
        //    {
        //        mainTimer = Convert.ToInt16(isActivatedMainTimer.FirstOrDefault().Value);
        //    }

        //    var gpsFrequencyRead = mainTimer;

        //    var isBusy = false;
        //    // Start a timer that depends of the frequency that is set in the database
        //    Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(gpsFrequencyRead), () =>
        //    {
        //        Xamarin.Forms.Device.BeginInvokeOnMainThread(async () =>
        //        {
        //            if (!isBusy)
        //            {
        //                isBusy = true;
        //                await MainMethodProcess();
        //                isBusy = false;
        //            }
        //        });

        //        return activeMainTimer;

        //    });
        //}

        //private void CheckIfLoadDataFromAPISuccesfully()
        //{
        //    if (this.failLoadZoneFromDB && this.failLoadParameterFromDB)
        //    {
        //        this.dialogService.Alert(
        //            "Información",
        //            "No se ha logrado conectar con nuestros servicios, ¿Desea intentarlo nuevamente?",
        //            "Reintentar",
        //            "Continuar",
        //            MainMethod().Wait
        //            );
        //    }
        //}

        //#endregion

        //#region Inicializations


        //private void LoadingText()
        //{
        //    this.SpeedLimitTextColor = "White";
        //    this.ActualSpeedTextColor = "White";
        //    this.BackgroundColor = "Black";
        //    this.ActualSpeedTitle = "";
        //    this.ActualSpeed = "";
        //    this.SpeedLimit = "";
        //    this.SpeedLimitTitle = Languages.Loading;
        //}

        //private void ActiveThreads(bool active)
        //{
        //    activeMainTimer = active;
        //}

        //#endregion

        //#region Principal Methods

        //private void LossGPSSignalMessage()
        //{
        //    if ((!CrossGeolocator.Current.IsGeolocationEnabled) && this.adviceGPSMessage)
        //    {
        //        this.adviceGPSMessage = false;
        //        this.ActualSpeed = "";
        //        this.ActualSpeedTitle = "";
        //        this.SpeedLimitTitle = "Sin Señal GPS";
        //        this.SpeedLimit = "";
        //        ScreenColorSettings("black", "red", "red");
        //        return;
        //    }
        //    else
        //    {
        //        this.adviceGPSMessage = true;
        //    }

        //}



        //private void ReadGPSActualData()
        //{
        //    try
        //    {
        //        //var stopFlag = this.lastLocation.Speed < Endpoint.MIN_SPEED_TO_CREATE_STOP ? 1 : 0;
        //        if (this.actualLocation != null && this.actualLocation.Latitude != 0 && this.actualLocation.Longitude != 0)
        //        {
        //            var distance = actualLocation.CalculateDistance(lastLocation, DistanceUnits.Kilometers) * 1000;

        //            if (distance < 1000 && actualLocation.Speed > 3)
        //            {
        //                this.gpsUserData = new GPSUserData
        //                {
        //                    //StopFlag = stopFlag,
        //                    Latitude = this.actualLocation.Latitude,
        //                    Longitude = this.actualLocation.Longitude,
        //                    Speed = this.actualLocation.Speed * 3.6,
        //                    DtmEvent = DateTime.UtcNow,
        //                    UserName = Settings.Email,
        //                    Distance = distance
        //                };
        //            }
        //            else
        //            {
        //                this.gpsUserData = new GPSUserData
        //                {
        //                    //StopFlag = stopFlag,
        //                    Latitude = this.actualLocation.Latitude,
        //                    Longitude = this.actualLocation.Longitude,
        //                    Speed = this.actualLocation.Speed * 3.6,
        //                    DtmEvent = DateTime.UtcNow,
        //                    UserName = Settings.Email,
        //                    Distance = 0
        //                };
        //            }

        //            this.lastLocation = this.actualLocation;

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "ReadGPSActualData";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //private async Task<bool> GPSUserDataCalculation()
        //{
        //    try
        //    {
        //        var isSuccess = false;
        //        await LoadGPSUserDataFromDB();
        //        isSuccess = await SendGPSUserDataToAPI();
        //        if (isSuccess)
        //        {
        //            await this.DeleteAllGPSUserData();
        //        }

        //        return isSuccess;
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "GPSUserDataCalculation";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //#endregion

        //#region Button Methods






        //private void IsActivatedParameter(string parameter, ref string data, ref bool isActivated)
        //{
        //    var isActivatedParameter = this.lstParameter.Where(q => q.Parameters == parameter);

        //    isActivated = isActivatedParameter.Count() == 0 ? false : true;

        //    if (isActivated)
        //    {
        //        data = isActivatedParameter.FirstOrDefault().Value;
        //    }
        //}

        //private async Task SendToAPIUsingFinishWorkhours(bool answer)
        //{
        //    var connection = await this.apiService.CheckConnection();

        //    if (!connection.IsSuccess)
        //    {
        //        var result = await Application.Current.MainPage.DisplayAlert(
        //                        Languages.Warning,
        //                        Languages.NoInternetConnectionAdvertising,
        //                        Languages.Retry,
        //                        Languages.SaveData);

        //        if (result)
        //        {
        //            await SendToAPIUsingFinishWorkhours(answer);
        //        }
        //        else
        //        {
        //            await FinishWorkhoursProcess();
        //        }
        //    }
        //    else if (answer)
        //    {
        //        await FinishWorkhoursProcess();
        //    }
        //}

        //private async Task FinishWorkhoursProcess()
        //{
        //    await SendAllDataToAPI();
        //    InitialSettings();
        //    await ExistDataPendingToSendTextShowing();


        //}

        //#endregion

        //#region API methods

        //private async Task<bool> LoadZonesFromAPIJustOnceToday()
        //{
        //    DateTime dateNow = DateTime.Now;
        //    bool result = false;

        //    var today = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day);

        //    if (Settings.LastLoadZonesUpdate.ToLocalTime() != today || Settings.LastBaseIdZones != Settings.BaseId)
        //    {
        //        result = await LoadZonesFromAPI();
        //        if (result)
        //        {
        //            Settings.LastLoadZonesUpdate = today;
        //            Settings.LastBaseIdZones = Settings.BaseId;
        //        }
        //        else
        //        {
        //            this.failLoadZoneFromDB = true;
        //        }
        //    }

        //    return result;
        //}

        //private async Task<bool> LoadParametersFromAPIJustOnceToday()
        //{
        //    DateTime dateNow = DateTime.Now;
        //    bool result = false;

        //    var today = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day);

        //    if (Settings.LastLoadParametersUpdate.ToLocalTime() != today || Settings.LastBaseIdParameters != Settings.BaseId)
        //    {
        //        result = await LoadParametersFromAPI();
        //        if (result)
        //        {
        //            Settings.LastLoadParametersUpdate = today;
        //            Settings.LastBaseIdParameters = Settings.BaseId;
        //        }
        //        else
        //        {
        //            this.failLoadParameterFromDB = true;
        //        }
        //    }

        //    return result;
        //}

        //private async Task<bool> LoadParameters()
        //{
        //    try
        //    {
        //        var response = false;
        //        this.failLoadParameterFromDB = false;

        //        var connection = await this.apiService.CheckConnection();

        //        if (connection.IsSuccess)
        //        {
        //            var answer = await this.LoadParametersFromAPIJustOnceToday();
        //            if (answer)
        //            {
        //                await this.SaveParametersToDB();
        //                response = true;
        //            }
        //            else
        //            {
        //                await this.LoadParametersFromDB();
        //                response = true;
        //            }
        //        }
        //        else
        //        {
        //            await this.LoadParametersFromDB();
        //            response = true;
        //            this.failLoadParameterFromDB = true;

        //        }

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "LoadParameters";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //public async Task<bool> LoadZones()
        //{
        //    try
        //    {
        //        var response = false;
        //        this.failLoadZoneFromDB = false;

        //        #region CheckInternetConnection
        //        var connection = await this.apiService.CheckConnection();

        //        if (connection.IsSuccess)
        //        {
        //            var answer = await this.LoadZonesFromAPIJustOnceToday();
        //            if (answer)
        //            {
        //                await this.SaveZonesToDB();
        //                if (this.lstZones != null || this.lstZones.Count > 0)
        //                {
        //                    await this.SaveZonesTypeToDB();
        //                }
        //                response = true;
        //            }
        //            else
        //            {
        //                await this.LoadZonesFromDB();
        //                await this.LoadZonesTypeFromDB();
        //                response = true;
        //            }
        //        }
        //        else
        //        {
        //            await this.LoadZonesFromDB();
        //            await this.LoadZonesTypeFromDB();
        //            this.failLoadZoneFromDB = true;
        //            response = true;
        //        }
        //        #endregion

        //        return response;

        //    }
        //    catch (System.Exception ex)
        //    {
        //        var methodName = "LoadZones";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //private async Task<bool> LoadParametersFromAPI()
        //{
        //    try
        //    {
        //        var Token = new
        //        {
        //            AccessToken = Settings.AccessToken,
        //            BaseId = Settings.BaseId,
        //            IsList = true
        //        };

        //        var response = await this.apiService.APIRequestPOST<Parameter>(
        //            Endpoint.URL_BASE,
        //            "/api",
        //            "/Syncronization/GetParameters",
        //            Token
        //            );

        //        if (!response.IsSuccess)
        //            return false;

        //        this.lstParameter = (List<Parameter>)response.Result;

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "LoadZones";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //private async Task<bool> LoadZonesFromAPI()
        //{
        //    try
        //    {
        //        var startLocation = await Geolocation.GetLocationAsync();
        //        var Token = new
        //        {
        //            AccessToken = Settings.AccessToken,
        //            BaseId = Settings.BaseId,
        //            IsList = true,
        //            Latitude = startLocation.Latitude,
        //            Longitude = startLocation.Longitude
        //        };

        //        var response = await this.apiService.APIRequestPOST<Models.Zone>(
        //            Endpoint.URL_BASE,
        //            "/api",
        //            "/Zones/GetZonesProvider",
        //            Token
        //            );

        //        if (!response.IsSuccess)
        //            return false;

        //        this.lstZones = (List<Zone>)response.Result;

        //        foreach (var zones in this.lstZones)
        //        {

        //            this.lstZonesType.Add(new ZoneType
        //            {
        //                Id = zones.ZoneTypes.FirstOrDefault().Id,
        //                ZoneId = zones.Id
        //            });

        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "LoadZonesFromAPI";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //private async Task<bool> SendEventToAPI()
        //{
        //    try
        //    {
        //        Response response = new Response
        //        {
        //            IsSuccess = false
        //        };

        //        var events = new List<Event>();
        //        if (this.lstEvent != null)
        //        {
        //            if (this.lstEvent.Count > 0)
        //            {
        //                events = this.lstEvent;

        //                foreach (var _event in events)
        //                {
        //                    var Token = new
        //                    {
        //                        AccessToken = Settings.AccessToken,
        //                        BaseId = Settings.BaseId,
        //                        _event.IdEvent,
        //                        _event.Distance,
        //                        _event.EventType,
        //                        UpDate = _event.FinishEvent.Value.ToString("yyyy-MM-dd HH:mm:ss"),
        //                        _event.ValueRef,
        //                        _event.ValueUser,
        //                        _event.IsNotified,
        //                        NotifiedDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
        //                        CreationDate = _event.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"),
        //                        _event.Latitude,
        //                        _event.Longitude
        //                    };

        //                    var connection = await this.apiService.CheckConnection();

        //                    if (connection.IsSuccess)
        //                    {
        //                        response = await this.apiService.APIRequestPOST<string>(
        //                        Endpoint.URL_BASE,
        //                        "/api",
        //                        "/Syncronization/SendEventToDB",
        //                        Token, false
        //                        );
        //                    }
        //                }
        //            }

        //        }


        //        return response.IsSuccess;
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "SendEventToAPI";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }
        //}

        //private async Task<bool> SendGPSUserDataToAPI()
        //{
        //    try
        //    {
        //        Response response = new Response
        //        {
        //            IsSuccess = false
        //        };
        //        //await this.dataService.DeleteAllGPSUserData();
        //        List<GPSUserData> gps = new List<GPSUserData>();
        //        if (this.lstGPSUserData != null)
        //        {
        //            if (this.lstGPSUserData.Count > 0)
        //                gps = this.lstGPSUserData;


        //            var data = gps.Select(q => new { q.Latitude, q.Longitude, q.Speed, q.DtmEvent, q.UserName, q.StopEnd, q.StopFlag, q.Distance }).Distinct().ToList();


        //            var lstGPS = new List<object>();

        //            foreach (var item in data)
        //            {
        //                var obGPS = new
        //                {
        //                    DtmEvent = item.DtmEvent.ToString("yyyy-MM-dd HH:mm:ss"),
        //                    Latitude = item.Latitude,
        //                    Longitude = item.Longitude,
        //                    Speed = item.Speed,
        //                    StopEnd = string.IsNullOrEmpty(item.StopEnd.ToString()) ? 0 : (int)item.StopEnd.Value,
        //                    StopFlag = string.IsNullOrEmpty(item.StopFlag.ToString()) ? 0 : (int)item.StopFlag.Value,
        //                    UserName = item.UserName,
        //                    Distance = item.Distance
        //                };

        //                lstGPS.Add(obGPS);
        //            }

        //            var Token = new
        //            {
        //                AccessToken = Settings.AccessToken,
        //                BaseId = Settings.BaseId,
        //                Data = lstGPS
        //            };

        //            var connection = await this.apiService.CheckConnection();

        //            if (connection.IsSuccess)
        //            {
        //                response = await this.apiService.APIRequestPOST<string>(
        //                Endpoint.URL_BASE,
        //                "/api",
        //                "/Syncronization/SendGPSUserDataToDB",
        //                Token, false
        //                );
        //            }
        //        }


        //        return response.IsSuccess;
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "SendGPSUserDataToAPI";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }
        //}


        //#endregion

        //#region CheckDeviceInsideZone Methods

        //private List<Zone> FilteringZonesMobileIsInside(Location location)
        //{
        //    try
        //    {
        //        try
        //        {
        //            this.GeohashMobile = GeoHash.Encode(location.Latitude, location.Longitude, Endpoint.PRECISION);

        //            var filteredZones = this.lstZones.Where(q => q.Hash.Contains(this.GeohashMobile)).ToList();

        //            //var ExistZoneUrban = this.lstZones.Where(q => q.ZoneTypes.Where(p => p.Id == "b1D").Count() > 0).ToList();

        //            //if(ExistZoneUrban.Count() > 0)
        //            //{
        //            //    foreach (var item in filteredZones)
        //            //    {
        //            //        item.IsRural = true;
        //            //    }
        //            //}

        //            if (filteredZones == null)
        //            {
        //                this.SpeedLimit = "NO DATA";
        //            }

        //            return filteredZones;
        //        }
        //        catch (Exception)
        //        {
        //            this.SpeedLimitTextColor = "White";
        //            this.ActualSpeedTextColor = "White";
        //            this.BackgroundColor = "Black";
        //            this.ActualSpeedTitle = Languages.ActualSpeedTitle;
        //            this.SpeedLimitTitle = Languages.SpeedLimitTitle;

        //            return new List<Zone>();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "FilteringZonesMobileIsInside";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}







        //private void NearToSpeedLimitVoiceNotification()
        //{
        //    if (this.activateNearSpeedLimitAdvice)
        //    {
        //        SpeakNow(Languages.SpeedWarning);
        //        this.activateNearSpeedLimitAdvice = false;
        //    }
        //}

        //private async Task SendEventToAPIProcess()
        //{
        //    try
        //    {
        //        var isActivatedSendData = this.lstParameter.Where(q => q.Parameters == Endpoint.ACTIVATE_SEND_DATA_WHEN_WORKHOURS_END);

        //        var isActivated = isActivatedSendData.Count() == 0 ? false : true;

        //        var sendData = Endpoint.DEFAULT_SEND_DATA;

        //        if (isActivated)
        //        {
        //            sendData = Convert.ToInt16(isActivatedSendData.FirstOrDefault().Value);
        //        }

        //        if (this.reduceSpeedAdvice && this.lstParameter.Where(q => q.Parameters == Endpoint.ACTIVATE_SAVE_EVENT).FirstOrDefault() != null)
        //        {
        //            this.reduceSpeedAdvice = false;
        //            await GetAllEventsPendingToSend();
        //            await GetTotalEventDistance();
        //            var response = await SendEventToAPI();
        //            if (response)
        //            {
        //                await UpdateToNofitifiedEvents();
        //                //await DeleteAllEvents();
        //            }

        //        }
        //        this.distanceList = new List<double>();


        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "SendEventToAPIProcess";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //private void GetEventMaxSpeed()
        //{
        //    this.firstSpeedEvent = Convert.ToInt32(this.ActualSpeed.Substring(0, 3));
        //    if (this.firstSpeedEvent > this.lastSpeedEvent)
        //    {
        //        this.lastSpeedEvent = this.firstSpeedEvent;
        //    }

        //}

        //private async Task GetTotalEventDistance()
        //{
        //    try
        //    {

        //        foreach (var _event in this.lstEvent)
        //        {
        //            var filteredGPSUserData = await GetFilteredGPSUserDataByEvent(_event.InitialGPSUserData.Value, _event.EndGPSUserData.Value);

        //            var totalSpeed = filteredGPSUserData.Select(q => q.Speed).Max();
        //            var totalDistance = CalculateDistance(filteredGPSUserData);
        //            //_event.ValueUser = totalSpeed;
        //            _event.Distance = totalDistance;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var text = ex;
        //    }

        //}

        //private async Task<List<GPSUserData>> GetFilteredGPSUserDataByEvent(int initialEventLocation, int endEventLocation)
        //{
        //    await LoadGPSUserDataFromDB();
        //    var filteredGPSUserDataByEvent = this.lstGPSUserData.Where(q => q.Id >= initialEventLocation && q.Id <= endEventLocation).ToList();
        //    return filteredGPSUserDataByEvent;
        //}

        //private double CalculateDistance(List<GPSUserData> filteredGPSUserDataByEvent)
        //{
        //    var lastLocation = new Location();
        //    var firstLocation = new Location();
        //    var totalDistance = 0.0;

        //    var isActivatedCalculateDistance = this.lstParameter.Where(q => q.Parameters == Endpoint.ACTIVATE_CALCULATE_DISTANCE);

        //    var isActivated = isActivatedCalculateDistance.Count() == 0 ? 0 : 1;

        //    if (isActivated > 0)
        //    {
        //        foreach (var gps in filteredGPSUserDataByEvent)
        //        {
        //            firstLocation = new Location { Latitude = gps.Latitude, Longitude = gps.Longitude, Speed = gps.Speed };

        //            if ((lastLocation.Longitude > 0 || lastLocation.Latitude > 0) && lastLocation.Speed > 3)
        //            {
        //                totalDistance += firstLocation.CalculateDistance(lastLocation, DistanceUnits.Kilometers) * 1000;
        //            }
        //            lastLocation = firstLocation;

        //        }
        //    }
        //    else
        //    {
        //        totalDistance = 0;
        //    }



        //    return totalDistance;
        //}



        //private void SpeedLimitVoiceNotification(int minimunSpeedLimitZone, bool entryToZone)
        //{
        //    try
        //    {
        //        if (entryToZone)
        //        {
        //            in_OutZone = true;

        //            if (this.lastSpeedLimit != this.SpeedLimit)
        //            {
        //                zoneEntryAdvice = true;
        //                this.lastSpeedLimit = this.SpeedLimit;

        //            }
        //            if (zoneEntryAdvice)
        //            {
        //                zoneEntryAdvice = false;
        //                SpeakNow(string.Format(Languages.EntryToSpeedLimitZone, minimunSpeedLimitZone.ToString()));
        //            }
        //        }
        //        else
        //        {
        //            zoneEntryAdvice = true;
        //            if (in_OutZone)
        //            {
        //                SpeakNow(string.Format(Languages.EntryToSpeedLimitZone, minimunSpeedLimitZone.ToString()));
        //                in_OutZone = false;
        //                //SpeakNow(Languages.ExitFromSpeedLimitZone);
        //                //SpeakNow(string.Format(Languages.EntryToSpeedLimitZone, minimunSpeedLimitZone.ToString()));
        //            }
        //            this.SpeedLimitTitle = Languages.SpeedLimitTitle;
        //            //this.SpeedLimit = "-- KM/H";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "SpeedLimitVoiceNotification";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }


        //}

        //private async Task<int> GetMinimunSpeedLimit(List<Zone> insideOfActualZones, int minimunSpeedLimitZone)
        //{
        //    try
        //    {
        //        foreach (var zone in insideOfActualZones)
        //        {

        //            try
        //            {
        //                var zoneType = this.lstZonesType.Where(q => q.ZoneId == zone.Id).FirstOrDefault();
        //                var zoneTypeId = zoneType.Id;
        //                var listParameter = await dataService.GetAllParameter(zoneTypeId);
        //                this.speedLimitZone = Convert.ToInt32(listParameter.First().Value.ToString());
        //                // Este limite se debe asignar segun el tipo de zona, y el limite esta en la tabla de parametros
        //                //this.speedLimitZone = 10 ; //Convert.ToInt32(zone.Name.Substring(zone.Name.Length - 2, 2));
        //            }
        //            catch (Exception)
        //            {
        //                this.speedLimitZone = 60;
        //            }

        //            if (speedLimitZone < minimunSpeedLimitZone)
        //            {
        //                minimunSpeedLimitZone = speedLimitZone;
        //            }
        //        }
        //        return minimunSpeedLimitZone;
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "GetMinimunSpeedLimit";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //private void AccumulateDistance()
        //{
        //    try
        //    {
        //        var distance = 0.0;
        //        if (this.actualLocation.Accuracy < 5)
        //        {
        //            distance = this.actualLocation.CalculateDistance(this.lastLocation, DistanceUnits.Kilometers) * 1000;
        //        }
        //        this.distanceList.Add(distance);
        //        this.distanceAccumulation = this.distanceList.Sum();
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "AccumulateDistance";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //private async Task SpeedExcessVoiceNotification()
        //{
        //    try
        //    {
        //        if (!this.reduceSpeedAdvice)
        //        {
        //            this.reduceSpeedAdvice = true;
        //            SpeakNow(Languages.SpeedExcess);
        //            await EventGeneration(startEvent: true);
        //        }
        //        else
        //        {
        //            await EventGeneration(startEvent: false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "SpeedExcessVoiceNotification";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //private async Task EventGeneration(bool startEvent)
        //{
        //    try
        //    {
        //        var initialGPSUserData = await this.dataService.GetMaxGPSUserDataId();
        //        if (startEvent)
        //        {
        //            var location = this.actualLocation;

        //            this.startEventModel = new Event
        //            {
        //                Distance = 0,
        //                EventType = Endpoint.SPEED_EXCESS_EVENT,
        //                IsNotified = 0,
        //                CreationDate = DateTime.UtcNow,
        //                FinishEvent = null,
        //                NotifiedDate = null,
        //                ValueRef = this.speedLimitZone,
        //                ValueUser = Convert.ToDouble(this.ActualSpeed.Substring(0, 3)),
        //                Latitude = location.Latitude,
        //                Longitude = location.Longitude,
        //                InitialGPSUserData = initialGPSUserData.Count > 0 ? initialGPSUserData.FirstOrDefault().Id : 0,
        //                EndGPSUserData = null
        //            };

        //            await SaveEventToDB(this.startEventModel);
        //        }
        //        else
        //        {
        //            //AccumulateDistance();

        //            var _event = new Event
        //            {
        //                IdEvent = this.startEventModel.IdEvent,
        //                Distance = 0,
        //                FinishEvent = DateTime.UtcNow,
        //                NotifiedDate = null,
        //                CreationDate = this.startEventModel.CreationDate,
        //                EventType = this.startEventModel.EventType,
        //                IsNotified = this.startEventModel.IsNotified,
        //                ValueRef = this.startEventModel.ValueRef,
        //                ValueUser = this.lastSpeedEvent,
        //                Latitude = this.startEventModel.Latitude,
        //                Longitude = this.startEventModel.Longitude,
        //                InitialGPSUserData = this.startEventModel.InitialGPSUserData,
        //                EndGPSUserData = initialGPSUserData.Count > 0 ? initialGPSUserData.FirstOrDefault().Id : 0
        //            };

        //            await UpdateEvent(_event);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "EventGeneration";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //#endregion

        //#region Calculation

        //private async Task DailyCalculation(short selector)
        //{
        //    try
        //    {
        //        var isActivatedSaveGPSLocation = this.lstParameter.Where(q => q.Parameters == Endpoint.ACTIVATE_SAVE_GPS_LOCATION);

        //        var isActivated = isActivatedSaveGPSLocation.Count() == 0 ? false : true;

        //        if (isActivated)
        //        {
        //            var totalDailyCalculation = new List<DailyUse>();
        //            double minDaily = 0;
        //            List<DailyUse> dailyIncomplete = new List<DailyUse>();
        //            short eventType = 0;

        //            dailyIncomplete.Add(new DailyUse
        //            {
        //                CalendarDay = DateTime.UtcNow
        //            });

        //            switch (selector)
        //            {
        //                case 1:
        //                    totalDailyCalculation = await this.dataService.GetAllDailyUse();
        //                    minDaily = Convert.ToDouble(lstParameter.Where(q => q.Parameters == Endpoint.MIN_USE_TIME).FirstOrDefault().Value);
        //                    //dailyIncomplete = totalDailyCalculation.Where(q => q.TotalReg < minDaily).ToList();
        //                    eventType = Endpoint.WORKHOUR_INCOMPLETE_EVENT;

        //                    break;
        //                case 2:
        //                    totalDailyCalculation = await this.dataService.GetDailyStop();
        //                    minDaily = Convert.ToDouble(lstParameter.Where(q => q.Parameters == Endpoint.MIN_STOPS).FirstOrDefault().Value);
        //                    //dailyIncomplete = totalDailyCalculation.Where(q => q.TotalReg < minDaily).ToList();
        //                    eventType = Endpoint.STOP_INCOMPLETE_EVENT;
        //                    break;
        //                case 3:
        //                    totalDailyCalculation = TotalDistanceByDayEvent();
        //                    minDaily = Convert.ToDouble(lstParameter.Where(q => q.Parameters == Endpoint.MIN_DISTANCE).FirstOrDefault().Value);
        //                    //dailyIncomplete = totalDailyCalculation.Where(q => q.TotalReg < minDaily).ToList();
        //                    eventType = Endpoint.TOTAL_DISTANCE_EVENT;
        //                    break;
        //                default:
        //                    break;
        //            }

        //            var existEvent = this.lstEvent.Where(q => q.ValueRef == minDaily).Count() == 1 ? true : false;
        //            if (dailyIncomplete.Any())
        //            {
        //                if (!existEvent)
        //                {

        //                    await InsertDailyEvent(totalDailyCalculation, eventType, minDaily);
        //                }
        //                else
        //                {
        //                    await UpdateDailyEvent(totalDailyCalculation, eventType, minDaily);
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "DailyCalculation";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //public async Task InsertDailyEvent(List<DailyUse> totalDailyCalculation, short eventType, double minDaily)
        //{
        //    foreach (var daily in totalDailyCalculation)
        //    {
        //        var _event = new Event
        //        {
        //            CreationDate = daily.CalendarDay < new DateTime(1900, 12, 1) ? new DateTime(1900, 12, 1) : daily.CalendarDay,
        //            Distance = 0,
        //            EventType = eventType,
        //            IsNotified = 0,
        //            NotifiedDate = null,
        //            FinishEvent = DateTime.UtcNow,
        //            ValueRef = minDaily,
        //            ValueUser = daily.TotalReg
        //        };

        //        this.lstEvent.Add(_event);
        //        await SaveEventToDB(_event);
        //    }
        //}

        //public async Task UpdateDailyEvent(List<DailyUse> totalDailyCalculation, short eventType, double minDaily)
        //{
        //    foreach (var daily in totalDailyCalculation)
        //    {
        //        await this.GetAllEventsPendingToSend();
        //        var eventUpdate = this.lstEvent.Where(q => q.ValueRef == minDaily).FirstOrDefault();

        //        var _event = new Event
        //        {
        //            FinishEvent = DateTime.UtcNow,
        //            ValueUser = daily.TotalReg
        //        };

        //        this.lstEvent.Where(q => q.ValueRef == minDaily).FirstOrDefault().ValueUser = _event.ValueUser;
        //        this.lstEvent.Where(q => q.ValueRef == minDaily).FirstOrDefault().FinishEvent = _event.FinishEvent;

        //        await UpdateEvent(_event);

        //    }
        //}

        //private async Task SendToAPIDailyCalculation(Event _event)
        //{
        //    var result = await SendEventToAPI();

        //    if (result)
        //    {
        //        await this.DeleteAllEvents();

        //        var _eventWithoutNotificate = await GetAllEventsPendingToSendByType(_event.EventType);

        //        _eventWithoutNotificate.FirstOrDefault().FinishEvent = DateTime.UtcNow;
        //        _eventWithoutNotificate.FirstOrDefault().NotifiedDate = DateTime.UtcNow;
        //        _eventWithoutNotificate.FirstOrDefault().IsNotified = 1;

        //        await UpdateToNofitifiedEvents();
        //    }
        //}

        //#endregion

        //#region SaveToDB Methods

        //private async Task SaveZonesToDB()
        //{
        //    try
        //    {
        //        await this.dataService.DeleteAllZones();
        //        await this.dataService.Insert(this.lstZones);
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "SaveZonesToDB";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //private async Task SaveZonesTypeToDB()
        //{
        //    try
        //    {
        //        await this.dataService.DeleteAllZonesType();
        //        await this.dataService.Insert(this.lstZonesType);
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "SaveZonesTypeToDB";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //private async Task SaveEventToDB(Event model)
        //{
        //    try
        //    {
        //        await this.dataService.Insert<Event>(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "SaveEventToDB";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }
        //}

        //private async Task SaveGPSUserDataToDB(int count)
        //{
        //    try
        //    {

        //        var isActivatedSaveGPSLocation = this.lstParameter.Where(q => q.Parameters == Endpoint.ACTIVATE_SAVE_GPS_LOCATION);

        //        var isActivated = isActivatedSaveGPSLocation.Count() == 0 ? false : true;

        //        if (count % Endpoint.EXCECUTE_SAVE_GPS_DATA == 0 && count > 0 && isActivated)
        //        {
        //            //if (this.activeSavingGPSDataToDB)
        //            //{
        //            //this.activeSavingGPSDataToDB = false;
        //            ReadGPSActualData();
        //            await this.dataService.Insert(this.gpsUserData);
        //            //}
        //        }
        //        else
        //        {
        //            //this.activeSavingGPSDataToDB = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "SaveGPSUserDataToDB";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //private async Task SaveParametersToDB()
        //{
        //    try
        //    {
        //        await this.dataService.DeleteAllParameter();
        //        await this.dataService.Insert(this.lstParameter);
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "SaveParametersToDB";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //#endregion

        //#region ReadFromDB Methods

        //private async Task ExistDataPendingToSendTextShowing()
        //{
        //    await LoadGPSUserDataFromDB();
        //    if (this.lstGPSUserData.Count > 0)
        //    {
        //        this.IsVisibleActualSpeedTitle = true;
        //        this.ActualSpeedTitle = Languages.DataPendingToSendAdvertizing;
        //        this.ActualSpeedTextColor = "Yellow";
        //    }
        //}

        //private async Task ExistDataPendingToSend()
        //{
        //    if (this.lstGPSUserData.Count > 0 && CrossConnectivity.Current.IsConnected)
        //    {

        //        await SendAllDataToAPI();
        //        this.SpeedLimitTitle = Languages.Loading;
        //    }
        //    else
        //    {
        //        this.ActualSpeedTextColor = "White";
        //        this.ActualSpeedTitle = "";
        //    }
        //}


        //private async Task SendAllDataToAPI()
        //{
        //    var isSuccessSendEvent = true;
        //    var isSuccessSendGPS = true;

        //    this.SpeedLimitTitle = Languages.Sending;
        //    this.IsVisibleSpeedLimitTitle = true;
        //    this.IsVisibleStartWorkHours = false;

        //    await DailyCalculation(Endpoint.DAILY_USAGE);
        //    await DailyCalculation(Endpoint.DAILY_STOPS);
        //    await DailyCalculation(Endpoint.DAILY_DISTANCE);

        //    if (this.lstEvent.Count > 0)
        //    {
        //        isSuccessSendEvent = await SendEventToAPI();
        //        if (isSuccessSendEvent)
        //        {
        //            await this.DeleteAllEvents();
        //        }
        //    }

        //    isSuccessSendGPS = await this.GPSUserDataCalculation();

        //    if (isSuccessSendGPS)
        //    {
        //        await Application.Current.MainPage.DisplayAlert(
        //            Languages.Success,
        //            Languages.SendDataSuccessProcess,
        //            Languages.Accept);
        //    }
        //    else
        //    {
        //        await Application.Current.MainPage.DisplayAlert(
        //             Languages.Success,
        //             Languages.SaveIntoStorage,
        //             Languages.Accept);

        //        //this.lstEvent = new List<Event>();
        //    }

        //    this.ActualSpeedTextColor = "White";
        //    this.ActualSpeedTitle = "";


        //}


        //private List<DailyUse> TotalDistanceByDayEvent()
        //{
        //    try
        //    {
        //        var totalDistanceResult = new List<DailyUse>();
        //        if (this.lstGPSUserData.Count > 0)
        //        {
        //            var days = this.lstGPSUserData.Select(q => q.DtmEvent.Date).Distinct().ToList();

        //            foreach (var day in days)
        //            {

        //                var startDay = new DateTime(day.Year, day.Month, day.Day);
        //                var endDay = startDay.AddDays(1);

        //                var isActivatedMinDistance = this.lstParameter.Where(q => q.Parameters == Endpoint.MIN_DISTANCE);

        //                var isActivated = isActivatedMinDistance.Count() == 0 ? false : true;

        //                var minDistanceUse = Endpoint.DEFAULT_MIN_DISTANCE;

        //                if (isActivated)
        //                {
        //                    minDistanceUse = Convert.ToDouble(isActivatedMinDistance.FirstOrDefault().Value);
        //                }


        //                var filteredGPSUserDataByDay = this.lstGPSUserData.Where(q => q.DtmEvent >= startDay && q.DtmEvent <= endDay).ToList();

        //                double totalDistanceByDay = CalculateDistance(filteredGPSUserDataByDay);
        //                var distance = Convert.ToInt32(Math.Round(totalDistanceByDay, 2));

        //                totalDistanceResult.Add(new DailyUse
        //                {
        //                    CalendarDay = day,
        //                    CalendarTime = day,
        //                    TotalReg = distance
        //                });

        //                //var _event = new Event
        //                //{
        //                //    CreationDate = DateTime.UtcNow,
        //                //    ValueUser = totalDistanceByDay,
        //                //    ValueRef = minDistanceUse,
        //                //    EventType = Endpoint.TOTAL_DISTANCE_EVENT,
        //                //    NotifiedDate = DateTime.UtcNow,
        //                //    Distance = 0,
        //                //    IsNotified = 0,
        //                //    InitialGPSUserData = null,
        //                //    EndGPSUserData =  null,
        //                //    FinishEvent = DateTime.UtcNow,
        //                //    Latitude = this.lstGPSUserData.LastOrDefault().Latitude,
        //                //    Longitude = this.lstGPSUserData.LastOrDefault().Longitude
        //                //};

        //                //this.lstEvent.Add(_event);

        //            }
        //        }
        //        return totalDistanceResult;
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "TotalDistanceByDayEvent";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }


        //}

        ////private async Task CheckTypeConnectionForSendData()
        ////{
        ////    var con = CrossConnectivity.Current.ConnectionTypes;
        ////    var connection = con.FirstOrDefault();

        ////    if (connection == Plugin.Connectivity.Abstractions.ConnectionType.Cellular)
        ////    {
        ////        var answer = await Application.Current.MainPage.DisplayAlert(
        ////                Languages.Warning,
        ////                Languages.AskMobileNetworkIsActivated,
        ////                Languages.Synchronize,
        ////                Languages.CancelTextButton);

        ////        if (answer)
        ////        {
        ////            await this.GPSUserDataCalculation();

        ////            await GetAllEventsPendingToSend();
        ////            if(this.lstEvent.Count > 0)
        ////            {
        ////                await SendEventToAPI();
        ////            }

        ////            this.ActualSpeedTextColor = "White";
        ////            this.ActualSpeedTitle = "";
        ////        }
        ////    }
        ////    else if (connection == Plugin.Connectivity.Abstractions.ConnectionType.WiFi)
        ////    {
        ////        await this.GPSUserDataCalculation();

        ////        await GetAllEventsPendingToSend();
        ////        if (this.lstEvent.Count > 0)
        ////        {
        ////            await SendEventToAPI();
        ////        }

        ////        this.ActualSpeedTextColor = "White";
        ////        this.ActualSpeedTitle = "";

        ////    }
        ////}

        //private async Task LoadZonesFromDB()
        //{
        //    try
        //    {
        //        this.lstZones = await this.dataService.GetAllZones();
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "LoadZonesFromDB";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }
        //}

        //private async Task LoadZonesTypeFromDB()
        //{
        //    try
        //    {
        //        this.lstZonesType = await this.dataService.GetAllZonesType();

        //        foreach (var zone in this.lstZones)
        //        {
        //            var zoneTypes = this.lstZonesType.Where(q => q.ZoneId == zone.Id).ToList();
        //            zone.ZoneTypes = zoneTypes;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "LoadZonesTypeFromDB";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }
        //}

        //private async Task LoadGPSUserDataFromDB()
        //{
        //    try
        //    {
        //        this.lstGPSUserData = await this.dataService.GetAllGPSUserData();
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "LoadGPSUserDataFromDB";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }
        //}

        //private async Task GetAllEventsPendingToSend()
        //{
        //    try
        //    {
        //        this.lstEvent = await this.dataService.GetAllEvent();

        //        this.lstEvent = this.lstEvent.Where(q => q.IsNotified == 0).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "GetAllEventsPendingToSend";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //private async Task<List<Event>> GetAllEventsPendingToSendByType(int EventType)
        //{
        //    try
        //    {
        //        var _event = await this.dataService.GetAllEvent(EventType);
        //        return _event.Where(q => q.IsNotified == 0).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "GetAllEventsPendingToSendByType";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}


        //private async Task LoadParametersFromDB()
        //{
        //    try
        //    {
        //        this.lstParameter = await this.dataService.GetAllParameter();

        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "LoadParametersFromDB";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }
        //}
        //#endregion

        //#region Updates Methods

        //private async Task UpdateEvent(Event model)
        //{
        //    try
        //    {
        //        if (model.IdEvent != 0)
        //        {
        //            await this.dataService.Update<Event>(model);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "UpdateEvent";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //private async Task UpdateToNofitifiedEvents()
        //{
        //    try
        //    {
        //        if (this.lstEvent != null)
        //        {
        //            if (this.lstEvent.Count > 0)
        //            {
        //                foreach (var _event in this.lstEvent)
        //                {
        //                    _event.IsNotified = 1;
        //                    _event.NotifiedDate = DateTime.UtcNow;
        //                    await UpdateEvent(_event);
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "UpdateToNofitifiedEvents";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}


        //#endregion

        //#region Delete Methods

        //private async Task DeleteAllGPSUserData()
        //{
        //    try
        //    {
        //        await this.dataService.DeleteAllGPSUserData();
        //        this.lstGPSUserData = new List<GPSUserData>();
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "DeleteAllGPSUserData";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }
        //}

        //private async Task DeleteAllEvents()
        //{
        //    try
        //    {
        //        await this.dataService.DeleteAllEvent();
        //        this.lstEvent = new List<Event>();
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "DeleteAllEvents";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }
        //}

        //#endregion

        //#endregion

        //#region Metodo para detenciones

        //private async Task LocateStops(int counter)
        //{
        //    try
        //    {
        //        var isActivatedSaveGPSLocation = this.lstParameter.Where(q => q.Parameters == Endpoint.ACTIVATE_SAVE_GPS_LOCATION);

        //        var isActivated = isActivatedSaveGPSLocation.Count() == 0 ? false : true;


        //        if (counter % Endpoint.EXCECUTE_STOP_DETECTION == 0 && counter > 0 && isActivated)
        //        {
        //            await LoadGPSUserDataFromDB();
        //            await LoadGPSEvaluatedFromDB();

        //            await SetFlagSpeedZero();
        //            bool existDetention = true;
        //            while (existDetention)
        //            {
        //                await UpdateIdDataSpeedZeroFromGPSEvaluated();
        //                existDetention = await SetStopEnd();
        //                await UpdateLastStop();
        //            }

        //            await this.dataService.Update(this.lstGPSUserData);
        //            this.counter = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "LocateStops";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }

        //}

        //private async Task SetFlagSpeedZero()
        //{
        //    try
        //    {
        //        var idDataSpeedCero = this.lstGPSEvaluated.Select(q => q.IdDataSpeedCero).FirstOrDefault();
        //        this.lstGPSUserData.Where(q => q.Id > idDataSpeedCero && q.Speed == 0 && q.StopFlag == null).ToList().ForEach(q => q.StopFlag = 0);
        //        await this.dataService.Update(this.lstGPSUserData);

        //    }
        //    catch (Exception ex)
        //    {
        //        var message = ex;
        //    }

        //}

        //private async Task UpdateIdDataSpeedZeroFromGPSEvaluated()
        //{
        //    try
        //    {
        //        var maxId = this.lstGPSUserData.Where(q => q.StopFlag == 0).Select(q => q.Id).Max();

        //        if (this.lstGPSEvaluated.Count > 0)
        //        {
        //            this.lstGPSEvaluated.FirstOrDefault().IdDataSpeedCero = maxId;
        //        }
        //        else
        //        {
        //            this.lstGPSEvaluated.Add(new GPSEvaluated
        //            {
        //                IdDataSpeedCero = maxId
        //            });
        //            await this.dataService.Insert(this.lstGPSEvaluated.LastOrDefault());
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        var message = ex;
        //    }

        //}


        //private async Task<bool> SetStopEnd()
        //{
        //    var existDetention = true;

        //    try
        //    {
        //        var idDataDetention = this.lstGPSEvaluated.Select(q => q.IdDataDetention).FirstOrDefault();

        //        var idsWhereSpeedZero = (from gps in this.lstGPSUserData
        //                                 where gps.StopFlag == 0 && gps.Id > idDataDetention
        //                                 select gps.Id);

        //        if (idsWhereSpeedZero.ToList().Count > 0)
        //        {
        //            var minIdWhereSpeedZero = idsWhereSpeedZero.Min();


        //            var IdsWhereSpeedHasValue = (from gps in this.lstGPSUserData
        //                                         where gps.Id > minIdWhereSpeedZero && gps.Speed.Value > 3 && gps.Distance > 2
        //                                         select gps.Id);

        //            if (IdsWhereSpeedHasValue.ToList().Count > 0)
        //            {
        //                var minIdWhereSpeedHasValue = IdsWhereSpeedHasValue.Min();

        //                var insertStopEndToGPSList = (from gps in this.lstGPSUserData
        //                                              where gps.Id >= minIdWhereSpeedZero && gps.Id < minIdWhereSpeedHasValue
        //                                              select gps.StopEnd = minIdWhereSpeedHasValue).ToList();

        //            }
        //            else
        //            {
        //                existDetention = false;
        //            }
        //        }
        //        else
        //        {
        //            existDetention = false;
        //        }

        //        return existDetention;
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = ex;
        //        return existDetention;
        //    }

        //}
        ////private async Task SetStopEnd()


        //private async Task UpdateLastStop()
        //{
        //    try
        //    {
        //        var idDetention = this.lstGPSEvaluated.Select(q => q.IdDataDetention).FirstOrDefault();

        //        var getStopEndAndId = (from gps in this.lstGPSUserData
        //                               where gps.Id > idDetention && gps.StopEnd != 0 && gps.StopEnd != null
        //                               select new
        //                               {
        //                                   gps.Id,
        //                                   gps.StopEnd
        //                               });

        //        if (getStopEndAndId.ToList().Count > 0)
        //        {
        //            var maxIdWithStopEnd = (int)getStopEndAndId.Max(q => q.StopEnd).Value;
        //            var updateIdDetention = (from gps in this.lstGPSEvaluated
        //                                     select gps.IdDataDetention = maxIdWithStopEnd);

        //            this.lstGPSEvaluated.FirstOrDefault().IdDataDetention = updateIdDetention.FirstOrDefault();

        //            await this.dataService.DeleteAllGPSEvaluated();
        //            await this.dataService.Insert(this.lstGPSEvaluated);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = ex;
        //    }


        //}

        //private async Task LoadGPSEvaluatedFromDB()
        //{
        //    try
        //    {
        //        this.lstGPSEvaluated = await this.dataService.GetAllGPSEvaluated();
        //    }
        //    catch (Exception ex)
        //    {
        //        var methodName = "LoadGPSEvaluated";
        //        var date = DateTime.UtcNow.ToString();
        //        this.SaveFile(Endpoint.FILE_NAME, date + " - " + "ERROR - " + methodName + " - " + ex.Message);
        //        throw ex;
        //    }
        //}

        //#endregion



    }
}
