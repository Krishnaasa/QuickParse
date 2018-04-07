
using System;
using System.Collections.Generic;

namespace QueryToJsonParserLibrary
{
    public class Profile
    {
        public string id { get; set; }
        public Name name { get; set; }
        public Location location { get; set; }
        public string imageId { get; set; }
        public List<Follower> followers { get; set; }
    }

    public class Name
    {
        private string firstName;

        public string first
        {
            get { return firstName; }
            set { firstName = value.Remove(value.IndexOf('<'),1); }
        }
        private string middleName;

        public string middle
        {
            get { return middleName; }
            set { middleName = value.Remove(value.IndexOf('<'),1); }
        }
        
        private string lastName;

        public string last
        {
            get { return lastName; }
            set { lastName = value.Remove(value.IndexOf('<'),1); }
        }
        
        
    }

    public class Location
    {
        private string locationName;

        public string name
        {
            get { return locationName; }
            set { locationName = value.Remove(value.IndexOf('<'), 1); }
        }
        public Coords coords { get; set; }
    }

    public class Coords
    {
        public Double @long { get; set; }
        public Double lat { get; set; }
    }

    public class Follower
    {
        public string id { get; set; }
        public string imageId { get; set; }
        public Name name { get; set; }
        public Location location { get; set; }
    }
    

}
