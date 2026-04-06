namespace Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    List<T> GetAll();
    T GetById(int id);
    T Create(T entity);
    T Update(T entity);

}
