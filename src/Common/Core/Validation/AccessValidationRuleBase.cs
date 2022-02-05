namespace Cofi.Validation;

public abstract class AccessValidationRuleBase
{
    IDictionary<string, object?>? _data;
    public IDictionary<string, object?> Data
    {
        get
        {
            if (_data is not null)
                return _data;

            _data = new Dictionary<string, object?>();
            SetData(_data);
            return _data;
        }
    }

    protected virtual void SetData(IDictionary<string, object?> data) { }
}
