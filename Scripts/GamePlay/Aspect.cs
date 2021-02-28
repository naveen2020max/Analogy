

public class Aspect
{
    public AspectType Type;
    public int Value;

    public Aspect(AspectType type)
    {
        Type = type;
    }
    public Aspect(int type)
    {
        Type = (AspectType)type;
    }

    public Aspect(int type, int value) : this(type)
    {
        Value = value;
    }

    public Aspect(AspectType type, int value) : this(type)
    {
        Value = value;
    }
}
