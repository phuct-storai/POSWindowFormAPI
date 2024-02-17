using POSWindowFormAPI.Controllers;
using POSWindowFormAPI.Data.Constants;
using POSWindowFormAPI.Data.Repositories.Interfaces;
using POSWindowFormAPI.Models;
using POSWindowFormAPI.Models.Request;
using POSWindowFormAPI.Services.Interfaces;
using System.Net;
using System.Security.Principal;

namespace POSWindowFormAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountRepository _accountRepository;
        private readonly ITrackingRepository _trackingRepository;
        public AccountService(ILogger<AccountController> logger,
                    IAccountRepository accountRepository,
                    ITrackingRepository trackingRepository)
        {
            _logger = logger;
            _accountRepository = accountRepository;
            _trackingRepository = trackingRepository;
        }

        public Account GetByUsername(string username)
        {
            return _accountRepository.GetByUsername(username);
        }
        public string CreateAccount(AuthenticationRequest authenticationRequest)
        { 
            //Add tracking
            Tracking tracking = new Tracking
            {
                ActionId = Guid.NewGuid().ToString(),
                Username = authenticationRequest.Username,
                ActionType = TrackingConstant.CREATE_ACCOUNT,
                ActionDetail = $"Create new account - {authenticationRequest.Username}",
                Date = DateTime.Now.ToString("G"),
                Status = string.Empty
            };

            var result = _accountRepository.CreateAccount(authenticationRequest);
            if (result == AuthenticaticalConstants.SUCCESS)
                tracking.Status = TrackingConstant.SUCCESS;
            else if (result)
                tracking.Status = TrackingConstant.FAILED;

            _trackingRepository.AddTracking(tracking);

            return tracking.Status;
        }

        public bool UpdateAccount(string username, AuthenticationRequest authenticationRequest)
        {
            //Add tracking
            Tracking tracking = new Tracking
            {
                ActionId = Guid.NewGuid().ToString(),
                Username = username,
                ActionType = TrackingConstant.UPDATE_ACCOUNT,
                ActionDetail = $"Update account - {username}",
                Date = DateTime.Now.ToString("G"),
                Status = string.Empty
            };
            if (_accountRepository.UpdateAccount(username, authenticationRequest))
            {
                tracking.Status = TrackingConstant.SUCCESS;
                _trackingRepository.AddTracking(tracking);
                return true;
            }
            else
            {
                tracking.Status = TrackingConstant.FAILED;
                _trackingRepository.AddTracking(tracking);
                return false;
            }
        }
        public bool DeleteAccount(string username)
        {
            //Add tracking
            Tracking tracking = new Tracking
            {
                ActionId = Guid.NewGuid().ToString(),
                Username = username,
                ActionType = TrackingConstant.DELETE_ACCOUNT,
                ActionDetail = $"Delete account - {username}",
                Date = DateTime.Now.ToString("G"),
                Status = string.Empty
            };
            //Delete Account
            if (_accountRepository.DeleteAccount(username))
            {
                tracking.Status = TrackingConstant.SUCCESS;
                _trackingRepository.AddTracking(tracking);
                return true; 
            }

            else
            {
                tracking.Status = TrackingConstant.FAILED;
                _trackingRepository.AddTracking(tracking);
                return false;
            }
        }

        public bool LoginValidate(string username, string password)
        {
            //Add tracking
            Tracking tracking = new Tracking
            {
                ActionId = Guid.NewGuid().ToString(),
                Username = username,
                ActionType = TrackingConstant.LOGIN,
                ActionDetail = $"Login account - {username}",
                Date = DateTime.Now.ToString("G"),
                Status = string.Empty
            };
            if (_accountRepository.LoginValidate(username, password))
            {
                tracking.Status = TrackingConstant.SUCCESS;
                _trackingRepository.AddTracking(tracking);
                return true;
            }

            else
            {
                tracking.Status = TrackingConstant.FAILED;
                _trackingRepository.AddTracking(tracking);
                return false;
            }

        }
    }
}
