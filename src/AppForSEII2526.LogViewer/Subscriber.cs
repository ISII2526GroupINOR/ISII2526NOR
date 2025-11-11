using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

public class Subscriber
{
    private readonly string _hostname = "10.81.139.157";
    private readonly string _queueName = "";
    private readonly string _exchangeName = "logs";

    private readonly string _userName = "guest"; 
    private readonly string _password = "guest";
    private readonly int _port = 5672;

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

        _channel.ExchangeDeclare(_exchangeName, ExchangeType.Fanout, true);

        var tempQueue = _channel.QueueDeclare();
        var _queueName = tempQueue.QueueName;

        _channel.QueueBind(queue: _queueName, exchange: _exchangeName, routingKey: "");
    }

    public void startConsuming()
    {
        var consumer = new EventingBasicConsumer(_channel);

        while(true){
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray(); //contenido del mensaje (array de bytes)
                var message = Encoding.UTF8.GetString(body); //se convierte de vuelta a string

                
                //var json = JsonSerializer.Deserialize(message);

                Console.WriteLine($"Pedido recibido: {message}");
            };

            _channel.BasicConsume(
                queue: _queueName,
                autoAck: true, // Confirmación automática de recepción del mensaje
                consumer: consumer
            ); 
        }
    }
}
