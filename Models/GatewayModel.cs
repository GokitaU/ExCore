using Core.EnumSpace;
using Core.MessageSpace;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Websocket.Client;

namespace Core.ModelSpace
{
  /// <summary>
  /// Interface that defines input and output processes
  /// </summary>
  public interface IGatewayModel : IStateModel
  {
    /// <summary>
    /// Production or Development mode
    /// </summary>
    EnvironmentEnum Mode { get; set; }

    /// <summary>
    /// Reference to the account
    /// </summary>
    IAccountModel Account { get; set; }

    /// <summary>
    /// Incoming data event
    /// </summary>
    ISubject<ITransactionMessage<IPointModel>> DataStream { get; }

    /// <summary>
    /// Send order event
    /// </summary>
    ISubject<ITransactionMessage<ITransactionOrderModel>> OrderSenderStream { get; }

    /// <summary>
    /// Get account data
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    Task<IAccountModel> GetAccount(IAccountModel account);

    /// <summary>
    /// Get history of instrument prices
    /// </summary>
    /// <param name="instrument"></param>
    /// <returns></returns>
    Task<IInstrumentModel> GetInstrument(IInstrumentModel instrument);

    /// <summary>
    /// Get prices
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    Task<IList<IPointModel>> GetPoints(IDictionary<dynamic, dynamic> inputs);

    /// <summary>
    /// Get orders
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    Task<IList<ITransactionOrderModel>> GetOrders(IDictionary<dynamic, dynamic> inputs);

    /// <summary>
    /// Get positions
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    Task<IList<ITransactionPositionModel>> GetPositions(IDictionary<dynamic, dynamic> inputs);

    /// <summary>
    /// Get options strikes
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    Task<IList<double>> GetOptionStrikes(IDictionary<dynamic, dynamic> inputs);

    /// <summary>
    /// Get options expiration dates
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    Task<IList<DateTime>> GetOptionExpirations(IDictionary<dynamic, dynamic> inputs);

    /// <summary>
    /// Get options chain
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    Task<IList<IInstrumentOptionModel>> GetOptionChains(IDictionary<dynamic, dynamic> inputs);

    /// <summary>
    /// Create orders
    /// </summary>
    /// <param name="orders"></param>
    /// <returns></returns>
    Task<IEnumerable<ITransactionOrderModel>> CreateOrders(params ITransactionOrderModel[] orders);

    /// <summary>
    /// Update orders
    /// </summary>
    /// <param name="orders"></param>
    /// <returns></returns>
    Task<IEnumerable<ITransactionOrderModel>> UpdateOrders(params ITransactionOrderModel[] orders);

    /// <summary>
    /// Close or cancel orders
    /// </summary>
    /// <param name="orders"></param>
    /// <returns></returns>
    Task<IEnumerable<ITransactionOrderModel>> DeleteOrders(params ITransactionOrderModel[] orders);
  }

  /// <summary>
  /// Implementation
  /// </summary>
  public abstract class GatewayModel : StateModel, IGatewayModel
  {
    /// <summary>
    /// Last quote
    /// </summary>
    protected IPointModel _point = null;

    /// <summary>
    /// HTTP client
    /// </summary>
    protected IClientService _serviceClient = null;

    /// <summary>
    /// Instance of the streaming client
    /// </summary>
    protected IWebsocketClient _streamClient = null;

    /// <summary>
    /// Unix time
    /// </summary>
    protected DateTime _unixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Socket connection options
    /// </summary>
    protected Func<ClientWebSocket> _streamOptions = new Func<ClientWebSocket>(() =>
    {
      return new ClientWebSocket
      {
        Options = { KeepAliveInterval = TimeSpan.FromSeconds(30) }
      };
    });

    /// <summary>
    /// Validation rules
    /// </summary>
    protected static TransactionOrderPriceValidation _orderRules = InstanceManager<TransactionOrderPriceValidation>.Instance;
    protected static InstrumentCollectionsValidation _instrumentRules = InstanceManager<InstrumentCollectionsValidation>.Instance;

    /// <summary>
    /// Production or Sandbox
    /// </summary>
    public EnvironmentEnum Mode { get; set; } = EnvironmentEnum.Sandbox;

    /// <summary>
    /// Reference to the account
    /// </summary>
    public virtual IAccountModel Account { get; set; }

    /// <summary>
    /// Incoming data event
    /// </summary>
    public virtual ISubject<ITransactionMessage<IPointModel>> DataStream { get; }

    /// <summary>
    /// Send order event
    /// </summary>
    public virtual ISubject<ITransactionMessage<ITransactionOrderModel>> OrderSenderStream { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    public GatewayModel()
    {
      DataStream = new Subject<ITransactionMessage<IPointModel>>();
      OrderSenderStream = new Subject<ITransactionMessage<ITransactionOrderModel>>();
    }

    /// <summary>
    /// Get account data
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    public abstract Task<IAccountModel> GetAccount(IAccountModel account);

    /// <summary>
    /// Get history of instrument prices
    /// </summary>
    /// <param name="instrument"></param>
    /// <returns></returns>
    public abstract Task<IInstrumentModel> GetInstrument(IInstrumentModel instrument);

    /// <summary>
    /// Get prices
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    public abstract Task<IList<IPointModel>> GetPoints(IDictionary<dynamic, dynamic> inputs);

    /// <summary>
    /// Get orders
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    public abstract Task<IList<ITransactionOrderModel>> GetOrders(IDictionary<dynamic, dynamic> inputs);

    /// <summary>
    /// Get positions
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    public abstract Task<IList<ITransactionPositionModel>> GetPositions(IDictionary<dynamic, dynamic> inputs);

    /// <summary>
    /// Get options strikes
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    public abstract Task<IList<double>> GetOptionStrikes(IDictionary<dynamic, dynamic> inputs);

    /// <summary>
    /// Get options expiration dates
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    public abstract Task<IList<DateTime>> GetOptionExpirations(IDictionary<dynamic, dynamic> inputs);

    /// <summary>
    /// Get options chain
    /// </summary>
    /// <param name="inputs"></param>
    /// <returns></returns>
    public abstract Task<IList<IInstrumentOptionModel>> GetOptionChains(IDictionary<dynamic, dynamic> inputs);

    /// <summary>
    /// Create orders
    /// </summary>
    /// <param name="orders"></param>
    /// <returns></returns>
    public abstract Task<IEnumerable<ITransactionOrderModel>> CreateOrders(params ITransactionOrderModel[] orders);

    /// <summary>
    /// Update orders
    /// </summary>
    /// <param name="orders"></param>
    /// <returns></returns>
    public abstract Task<IEnumerable<ITransactionOrderModel>> UpdateOrders(params ITransactionOrderModel[] orders);

    /// <summary>
    /// Close or cancel orders
    /// </summary>
    /// <param name="orders"></param>
    /// <returns></returns>
    public abstract Task<IEnumerable<ITransactionOrderModel>> DeleteOrders(params ITransactionOrderModel[] orders);

    /// <summary>
    /// Ensure that each series has a name and can be attached to specific area on the chart
    /// </summary>
    /// <param name="model"></param>
    protected bool EnsureOrderProps(params ITransactionOrderModel[] models)
    {
      var errors = new List<ValidationFailure>();

      foreach (var model in models)
      {
        errors.AddRange(_orderRules.Validate(model).Errors);
        errors.AddRange(_instrumentRules.Validate(model.Instrument).Errors);
        errors.AddRange(model.Orders.SelectMany(o => _orderRules.Validate(o).Errors));
        errors.AddRange(model.Orders.SelectMany(o => _instrumentRules.Validate(o.Instrument).Errors));
      }

      foreach (var error in errors)
      {
        InstanceManager<LogService>.Instance.Log.Error(error.ErrorMessage);
      }

      return errors.Any() == false;
    }

    /// <summary>
    /// Update missing values of a data point
    /// </summary>
    /// <param name="point"></param>
    protected virtual IPointModel UpdatePointProps(IPointModel point)
    {
      point.Account = Account;
      point.Name = point.Instrument.Name;
      point.ChartData = point.Instrument.ChartData;
      point.TimeFrame = point.Instrument.TimeFrame;

      UpdatePoints(point);

      var message = new TransactionMessage<IPointModel>
      {
        Action = ActionEnum.Create,
        Next = point.Instrument.PointGroups.LastOrDefault()
      };

      DataStream.OnNext(message);

      return point;
    }

    /// <summary>
    /// Update collection with points
    /// </summary>
    /// <param name="point"></param>
    protected virtual IPointModel UpdatePoints(IPointModel point)
    {
      point.Instrument.Points.Add(point);
      point.Instrument.PointGroups.Add(point);

      return point;
    }
  }

  /// <summary>
  /// Validation rules
  /// </summary>
  public class GatewayValidation : AbstractValidator<IGatewayModel>
  {
    public GatewayValidation()
    {
      RuleFor(o => o.Name).NotNull().NotEmpty().WithMessage("No name");
    }
  }
}
