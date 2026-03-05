namespace MonoGameLibrary.StatefulEvent;

public interface IValue<T>
{
    bool Equals(T other);
}
