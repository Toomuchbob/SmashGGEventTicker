using System;
using System.Collections.Generic;

namespace SmashGGEventTicker
{
    public class SmashGGResponse
    {
        public Data Data { get; set; }
    }
    public class Data
    {
        public Tournaments Tournaments { get; set; }
    }

    public class Tournaments
    {
        public List<Tournament> Nodes { get; set; }
    }

    public class Tournament
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public bool IsOnline { get; set; }
        public string Slug { get; set; }
        public uint StartAt { get; set; }
        public bool IsRegistrationOpen { get; set; }
        public string Hashtag { get; set; }
        public List<Event> Events { get; set; }
        public List<Stream> Streams { get; set; }
    }

    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isOnline { get; set; }
        public int? NumEntrants { get; set; }
        public int? EntrantSizeMax { get; set; }
        public uint StartAt { get; set; }
        public Videogame Videogame { get; set; }
    }

    public class Videogame
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Stream
    {
        public int Id { get; set; }
        public bool IsOnline { get; set; }
        public string StreamName { get; set; }
        public string StreamSource { get; set; }
    }

}
