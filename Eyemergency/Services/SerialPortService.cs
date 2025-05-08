using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

public class SerialPortService : IHostedService
{
    private readonly List<SerialPort> _serialPorts = new List<SerialPort>();
    private readonly string[] _portNames = { "COM5", "COM6", "COM7" };
    private readonly int _baudRate = 9600;

    public static string LatestTagId { get; private set; } = "";

    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (var portName in _portNames)
        {
            try
            {
                var serialPort = new SerialPort(portName, _baudRate);
                serialPort.DataReceived += SerialPort_DataReceived;
                serialPort.Open();
                _serialPorts.Add(serialPort);

                Console.WriteLine($"Serial port {portName} opened successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to open serial port {portName}: {ex.Message}");
            }
        }

        return Task.CompletedTask;
    }

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        if (sender is SerialPort serialPort)
        {
            try
            {
                string data = serialPort.ReadLine().Trim();
                LatestTagId = data;
                Console.WriteLine($"[{serialPort.PortName}] Received Tag ID: {data}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from {serialPort.PortName}: {ex.Message}");
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var serialPort in _serialPorts)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                Console.WriteLine($"Serial port {serialPort.PortName} closed.");
            }
        }

        return Task.CompletedTask;
    }
}
