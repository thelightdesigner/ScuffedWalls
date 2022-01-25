using System;
using System.Collections.Generic;
using System.Text;

namespace ModChart
{
    public static class Heck
    {

        public const string
               _height = "_height",
               _attenuation = "_attenuation",
               _offset = "_offset",
               _startY = "_startY",
               _position = "_position",
               _localPosition = "_localPosition",
               _scale = "_scale",
               _localRotation = "_localRotation",
               _rotation = "_rotation",
               _definitePosition = "_definitePosition",
               _dissolve = "_dissolve",
               _dissolveArrow = "_dissolveArrow",
               _time = "_time",
               _track = "_track",
               _color = "_color",
               _id = "_id",
               _data = "_data",
               _active = "_active",
               _lookupMethod = "_lookupMethod",
               _animation = "_animation",
               _duplicate = "_duplicate",
               _customEvents = "_customEvents",
               _pointDefinitions = "_pointDefinitions",
               _worldpositionstays = "_worldpositionstays",
               _bookmarks = "_bookmarks",
               _environment = "_environment",
               _BPMChanges = "_BPMChanges",
               AnimateTrack = "AnimateTrack",
               AssignPathAnimation = "AssignPathAnimation",
               AssignPlayerToTrack = "AssignPlayerToTrack",
               AssignTrackParent = "AssignTrackParent",
               AssignFogTrack = "AssignFogTrack";
        public static string[] NoodleExtensionsPropertyNames => new string[]
        {
            _height,
            _attenuation,
            _position,
            _rotation,
            _scale,
            _localPosition,
            _localRotation,
            _dissolve,
            _dissolveArrow,
            _time
        };

        public static void Append(ICustomDataMapObject MapObject, ICustomDataMapObject AppendObject, AppendPriority type)
        {
            switch (type)
            {
                case AppendPriority.Low:
                    foreach (var property in MapObject.GetType().GetProperties())
                        if (property.GetValue(MapObject) == null)
                            property.SetValue(MapObject, property.GetValue(AppendObject));

                    if (AppendObject._customData != null)
                    {
                        MapObject._customData = TreeDictionary.Merge(
                            MapObject._customData,
                            AppendObject._customData,
                            TreeDictionary.MergeType.Dictionaries | TreeDictionary.MergeType.Objects,
                            TreeDictionary.MergeBindingFlags.HasValue);
                    }
                    break;
                case AppendPriority.High:
                    foreach (var property in MapObject.GetType().GetProperties())
                        if (property.GetValue(AppendObject) != null)
                            property.SetValue(MapObject, property.GetValue(AppendObject));

                    if (MapObject._customData != null)
                    {
                        MapObject._customData = TreeDictionary.Merge(
                            AppendObject._customData,
                            MapObject._customData,
                            TreeDictionary.MergeType.Dictionaries | TreeDictionary.MergeType.Objects,
                            TreeDictionary.MergeBindingFlags.HasValue);
                    }
                    break;
            }
        }
        public enum AppendPriority
        {
            Low,
            High
        }
    }
}
