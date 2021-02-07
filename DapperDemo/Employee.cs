namespace DapperDemo
{
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }


        public override bool Equals(object obj)
        {
            if (!(obj is Employee))
                return false;

            return FirstName == ((Employee)obj).FirstName && LastName == ((Employee)obj).LastName;
        }

        public override int GetHashCode()
        {
            throw new System.NotImplementedException();
        }
    }
}