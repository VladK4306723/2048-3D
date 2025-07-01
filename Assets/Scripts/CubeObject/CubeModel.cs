public class CubeModel
{
    public int Po2Value { get; private set; }

    public CubeModel(int value)
    {
        Po2Value = value;
    }

    public void SetValue(int value)
    {
        Po2Value = value;
    }
}