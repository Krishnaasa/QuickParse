using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace QueryToJsonParserLibrary
{
    public class CoreParserLogic
    {
        public Profile GetJson(string queryData)
        {
            Profile wholeProfile = null;

            string[] nestedData = queryData.Split(
                 new string[] { "**" }, StringSplitOptions.None);
            
            foreach (var item in nestedData)
            {
                if (item.StartsWith("profile"))
                {
                    //Assuming every prfile holder is a Follower and hence we call GetCommonData to get all the 
                    //common properties
                    Follower follower = GetCommonData(new Follower(), item.Split('|').Skip(1));
                    wholeProfile = new Profile()
                    {
                        id = follower.id,
                        imageId = follower.imageId,
                        name = follower.name,
                        location = follower.location,
                        followers = new List<Follower>()
                    };
                }

                if (item.StartsWith("followers"))
                {
                    //First finding number of followers
                    IEnumerable<string> listOfFollowers = item.Split(new string[] {"@@" }, StringSplitOptions.None);

                    foreach (string fellowFollower in listOfFollowers)
                    {
                        Follower follower;
                        if (item.StartsWith("followers"))
                        {
                            follower = GetCommonData(new Follower(), fellowFollower.Split('|').Skip(1));
                        }
                         follower = GetCommonData(new Follower(), fellowFollower.Split('|').Skip(1));

                        if (wholeProfile != null)
                        {
                            wholeProfile.followers.Add(follower); //Adding each follower to ListOfFollowers
                        }
                    }
                    
                }
            }
            
            return wholeProfile;
        }
        /// <summary>
        /// Gets the complete details of a follower
        /// </summary>
        /// <param name="partialProfile">Passing instance object of Follower</param>
        /// <param name="profileData">Refers to each profile structure from querystring</param>
        /// <returns>An object of Follower</returns>
        private Follower GetCommonData(Follower partialProfile, IEnumerable<string> profileData)
        {
            foreach (string objectData in profileData)
            {

                Type identifiedDataType = IdentifyObjectType(objectData);

                if (identifiedDataType == typeof(string))
                {
                    if (objectData.EndsWith("jpg")) //Sets to imageid if string ends with jpg
                    {
                        partialProfile.imageId = ReturnIdElement(objectData);
                    }
                    else
                    {
                        partialProfile.id = ReturnIdElement(objectData);
                    }
                }
                if (identifiedDataType == typeof(Name))
                {
                    partialProfile.name = ReturnNameElement(objectData);
                }
                if (identifiedDataType == typeof(Location))
                {
                    partialProfile.location = ReturnLocationElement(objectData);
                }

            }
            return partialProfile;
        }


        /// <summary>
        /// Identifies each string based on the RegEx pattern to determine which string belongs to 
        /// which similar pattern of object.
        /// </summary>
        /// <param name="objectData"></param>
        /// <returns>Type of object if Regex Passes else a null</returns>
        private Type IdentifyObjectType(string objectData)
        {
            if (Regex.IsMatch(objectData, "^[0-9][0-9]*"))
            {
                return typeof(string);
            }
            if (Regex.IsMatch(objectData, "^(<)([A-Za-z0-9])*><([A-Za-z0-9])*><([A-Za-z0-9])*>"))
            {
                return typeof(Name);
            }
            if (Regex.IsMatch(objectData, "^(<)([A-Za-z0-9])*><<([0-9]*[.]?[0-9]*)><([0-9]*[.]?[0-9]*)>>"))
            {
                return typeof(Location);
            }
            return null;
        }

        /// <summary>
        /// Identifies type of string
        /// </summary>
        /// <param name="data"></param>
        /// <returns>string passed as parameter</returns>
        private string ReturnIdElement(string data)
        {
            return data;
        }
        /// <summary>
        /// Parses the XML structure to a Name based object
        /// </summary>
        /// <param name="data">Resembles same as "<Aamir><hussain><>" from input</param>
        /// <returns></returns>
        private Name ReturnNameElement(string data)
        {
            Name localName = new Name();
            string x = data.Replace('>', ',');
            string[] partsOfName = x.Split(',');
            localName.first = partsOfName[0];
            localName.middle = partsOfName[1];
            localName.last = partsOfName[2];

            return localName;
        }
        /// <summary>
        /// Parses the XML structure to a Location based object
        /// </summary>
        /// <param name="data">Resembles same as <Mumbai><<72.872075><19.075606>></param>
        /// <returns></returns>
        private Location ReturnLocationElement(string data)
        {
            Location location = new Location();
            string locationAndCoords = data.Substring(data.IndexOf("<<"));

            string x = data.Replace('>', ',');
            string[] partsOfLocation = x.Split(',');
            location.name = partsOfLocation[0];
            location.coords = ReturnCoordsElement(locationAndCoords);

            return location;
        }
        /// <summary>
        /// Parses the XML structure to a Location based object
        /// </summary>
        /// <param name="data">Resembles same as <<72.872075><19.075606>></param>
        /// <returns></returns>
        private Coords ReturnCoordsElement(string data)
        {
            Coords coordinates = new Coords();
            string x = data.Replace('>', ',');
            string[] coordinateElements = x.Split(',');

            string longitude = coordinateElements[0].Remove(coordinateElements[0].IndexOf('<'), 2);
            Double longitudeF = 0;
            if (Double.TryParse(longitude, out longitudeF))
            {
                coordinates.@long = longitudeF;
            }

            string latitude = coordinateElements[1].Remove(coordinateElements[0].IndexOf('<'), 1);
            Double latitudeF = 0;

            if (Double.TryParse(latitude, out latitudeF))
            {
                coordinates.lat = latitudeF;
            }
            return coordinates;
        }

    }
}