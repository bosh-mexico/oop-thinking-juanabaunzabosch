using System;

namespace VehicleSpeedGovernor
{
    // Holds and manages speed threshold configuration
    public class SpeedConfig
    {
        public int Threshold { get; private set; }

        public SpeedConfig(int initialThreshold)
        {
            SetThreshold(initialThreshold);
        }

        public void SetThreshold(int threshold)
        {
            if (threshold <= 0)
                throw new ArgumentException("Invalid threshold!");

            Threshold = threshold;
            Console.WriteLine($"[CONFIG] Threshold set to {Threshold} km/h");
        }
    }

    // Represents vehicle data like current speed
    public class VehicleState
    {
        public int CurrentSpeed { get; private set; }

        public void UpdateSpeed(int speed)
        {
            CurrentSpeed = speed;
            Console.WriteLine($"[DATA] Vehicle speed updated: {CurrentSpeed} km/h");
        }
    }

    // Handles interaction with ECU to override throttle
    public class EcuController
    {
        public void OverrideAcceleration()
        {
            Console.WriteLine("[ECU] Acceleration overridden to maintain threshold.");
        }
    }

    // Responsible for driver alerts and feedback
    public class DriverFeedback
    {
        public void ProvideAlert()
        {
            Console.WriteLine("[ALERT] Speed limit reached!");
            Console.Beep();
        }
    }

    // Handles logging of events
    public class Logger
    {
        public void LogGovernorActivation(int currentSpeed, int threshold)
        {
            Console.WriteLine($"[LOG] Governor activated at speed {currentSpeed} km/h (Threshold: {threshold})");
        }
    }

    // Main class orchestrating the components
    public class VehicleSpeedGovernor
    {
        private readonly SpeedConfig _config;
        private readonly VehicleState _vehicle;
        private readonly EcuController _ecu;
        private readonly DriverFeedback _feedback;
        private readonly Logger _logger;

        private bool _isGovernorActive;

        public VehicleSpeedGovernor(int initialThreshold)
        {
            _config = new SpeedConfig(initialThreshold);
            _vehicle = new VehicleState();
            _ecu = new EcuController();
            _feedback = new DriverFeedback();
            _logger = new Logger();
            _isGovernorActive = false;
        }

        public void SetSpeedThreshold(int threshold)
        {
            _config.SetThreshold(threshold);
        }

        public void UpdateVehicleSpeed(int newSpeed)
        {
            _vehicle.UpdateSpeed(newSpeed);
            CheckAndEnforceSpeedLimit();
        }

        private void CheckAndEnforceSpeedLimit()
        {
            if (_vehicle.CurrentSpeed > _config.Threshold)
            {
                if (!_isGovernorActive)
                {
                    _isGovernorActive = true;
                    _ecu.OverrideAcceleration();
                    _feedback.ProvideAlert();
                    _logger.LogGovernorActivation(_vehicle.CurrentSpeed, _config.Threshold);
                }
            }
            else
            {
                _isGovernorActive = false;
            }
        }

        public void ShowStatus()
        {
            Console.WriteLine($"[STATUS] Current Speed: {_vehicle.CurrentSpeed}, Threshold: {_config.Threshold}, Governor Active: {_isGovernorActive}");
        }
    }
}
