using System;
using System.Text;
using RabbitMQ.Client;

public class Subscriber
{
    private readonly string _hostname = "localhost";
    private readonly string _queueName = "pedidos";
    private readonly string _exchangeName = "exchange";

    private readonly string _userName = ""; 
    private readonly string _password = "";
    private readonly int _port = -1;

    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IBasicProperties _properties;

    public Subscriber()
	{
        var factory = new ConnectionFactory()
        {
            HostName = _hostname,
            UserName = _userName,
            Password = _password,
            Port = _port
        };
        _connection = factory.CreateConnection();

        _channel = _connection.CreateModel();
        _properties = _channel.CreateBasicProperties();
        _properties.Persistent = true; // Hace el mensaje persistente

        _channel.ExchangeDeclare(_exchangeName, ExchangeType.fanout);

        var tempQueue = _channel.QueueDeclare();
        var _queueName = tempQueue.QueueName;

        _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");
    }
}
