namespace CoreApp.Infrastructure.ShareKernel
{
    public abstract class DomainEntity<T>
    {
        public T Id { get; set; }

        /// <summary>
        /// True if domain entity has identity
        /// </summary>
        /// <returns></returns>
        public bool IsTransient()
        {
            return Id.Equals(default(T));
        }
    }
}
