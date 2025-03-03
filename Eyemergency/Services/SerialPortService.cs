using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

public class SerialPortService : IHostedService
{
    private SerialPort _serialPort;
    private readonly string _portName = "COM6"; 
    private readonly int _baudRate = 9600;

    public static string LatestTagId { get; private set; } = "";

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _serialPort = new SerialPort(_portName, _baudRate);
        _serialPort.DataReceived += SerialPort_DataReceived;
        _serialPort.Open();
        Console.WriteLine("Serial port opened.");
        return Task.CompletedTask;
    }

    private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
    {
        string data = _serialPort.ReadLine().Trim(); // Read the tag ID
        LatestTagId = data;
        Console.WriteLine("Received Tag ID: " + data);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _serialPort.Close();
        Console.WriteLine("Serial port closed.");
        return Task.CompletedTask;
    }
}