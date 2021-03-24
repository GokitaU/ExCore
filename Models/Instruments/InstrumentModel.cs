using Core.CollectionSpace;
using FluentValidation;
using System;

namespace Core.ModelSpace
{
  /// <summary>
  /// Generic instrument definition
  /// </summary>
  public interface IInstrumentModel : IBaseModel
  {
    /// <summary>
    /// Volume
    /// </summary>
    double? Volume { get; set; }

    /// <summary>
    /// Open interest
    /// </summary>
    double? OpenInterest { get; set; }

    /// <summary>
    /// Long swap rate for keeping position overnight
    /// </summary>
    double? SwapLong { get; set; }

    /// <summary>
    /// Short swap rate for keeping position overnight
    /// </summary>
    double? SwapShort { get; set; }

    /// <summary>
    /// Leverage or margin rate
    /// </summary>
    double? Leverage { get; set; }

    /// <summary>
    /// Commission
    /// </summary>
    double? Commission { get; set; }

    /// <summary>
    /// Contract size for 1 trading lot in currencies and futures
    /// </summary>
    double? ContractSize { get; set; }

    /// <summary>
    /// Tick size, i.e. minimum price change 
    /// </summary>
    double? StepSize { get; set; }

    /// <summary>
    /// Tick value, i.e. how much price change within one tick
    /// </summary>
    double? StepValue { get; set; }

    /// <summary>
    /// Aggregation period for the quotes
    /// </summary>
    TimeSpan? TimeFrame { get; set; }

    /// <summary>
    /// Reference to the account
    /// </summary>
    IAccountModel Account { get; set; }

    /// <summary>
    /// Style
    /// </summary>
    IChartDataModel ChartData { get; set; }

    /// <summary>
    /// Current price data
    /// </summary>
    IPointModel Point { get; set; }

    /// <summary>
    /// List of all ticks from the server
    /// </summary>
    IIndexCollection<IPointModel> Points { get; set; }

    /// <summary>
    /// List of all ticks from the server aggregated into bars
    /// </summary>
    IIndexCollection<IPointModel> PointGroups { get; set; }
  }

  /// <summary>
  /// Generic instrument definition
  /// </summary>
  public class InstrumentModel : BaseModel, IInstrumentModel
  {
    /// <summary>
    /// Volume
    /// </summary>
    public virtual double? Volume { get; set; }

    /// <summary>
    /// Open interest
    /// </summary>
    public virtual double? OpenInterest { get; set; }

    /// <summary>
    /// Long swap rate for keeping position overnight
    /// </summary>
    public virtual double? SwapLong { get; set; }

    /// <summary>
    /// Short swap rate for keeping position overnight
    /// </summary>
    public virtual double? SwapShort { get; set; }

    /// <summary>
    /// Leverage or margin rate
    /// </summary>
    public virtual double? Leverage { get; set; }

    /// <summary>
    /// Commission
    /// </summary>
    public virtual double? Commission { get; set; }

    /// <summary>
    /// Contract size for 1 trading lot in currencies and futures
    /// </summary>
    public virtual double? ContractSize { get; set; }

    /// <summary>
    /// Tick size, i.e. minimum price change 
    /// </summary>
    public virtual double? StepSize { get; set; }

    /// <summary>
    /// Tick value, i.e. how much price change within one tick
    /// </summary>
    public virtual double? StepValue { get; set; }

    /// <summary>
    /// Aggregation period for the quotes
    /// </summary>
    public virtual TimeSpan? TimeFrame { get; set; }

    /// <summary>
    /// Reference to the account
    /// </summary>
    public virtual IAccountModel Account { get; set; }

    /// <summary>
    /// Style
    /// </summary>
    public virtual IChartDataModel ChartData { get; set; }

    /// <summary>
    /// Current price data
    /// </summary>
    public virtual IPointModel Point { get; set; }

    /// <summary>
    /// List of all ticks from the server
    /// </summary>
    public virtual IIndexCollection<IPointModel> Points { get; set; }

    /// <summary>
    /// List of all ticks from the server aggregated into bars
    /// </summary>
    public virtual IIndexCollection<IPointModel> PointGroups { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public InstrumentModel()
    {
      SwapLong = 0.0;
      SwapShort = 0.0;
      StepSize = 0.01;
      StepValue = 0.01;
      Commission = 0.0;
      ContractSize = 1.0;

      Point = new PointModel();
      ChartData = new ChartDataModel();
      Points = new TimeSpanCollection<IPointModel>();
      PointGroups = new TimeSpanCollection<IPointModel>();
    }
  }

  /// <summary>
  /// Validation rules
  /// </summary>
  public class InstrumentValidation : AbstractValidator<IInstrumentModel>
  {
    public InstrumentValidation()
    {
      RuleFor(o => o.SwapLong).NotNull().WithMessage("No long swap");
      RuleFor(o => o.SwapShort).NotNull().WithMessage("No short swap");
      RuleFor(o => o.Commission).NotNull().WithMessage("No commission");
      RuleFor(o => o.ContractSize).NotNull().NotEqual(0).WithMessage("No contract size");
      RuleFor(o => o.StepSize).NotNull().NotEqual(0).WithMessage("No point size");
      RuleFor(o => o.StepValue).NotNull().NotEqual(0).WithMessage("No point value");
      RuleFor(o => o.TimeFrame).NotNull().WithMessage("No time frame");
      RuleFor(o => o.Points).NotNull().WithMessage("No points");
      RuleFor(o => o.PointGroups).NotNull().WithMessage("No point groups");
    }
  }

  /// <summary>
  /// Validation rules
  /// </summary>
  public class InstrumentCollectionsValidation : AbstractValidator<IInstrumentModel>
  {
    public InstrumentCollectionsValidation()
    {
      Include(new InstrumentValidation());

      RuleFor(o => o.Points).NotNull().NotEmpty().WithMessage("No points");
      RuleFor(o => o.PointGroups).NotNull().NotEmpty().WithMessage("No point groups");
    }
  }
}
