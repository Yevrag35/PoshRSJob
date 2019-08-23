using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MG.Jobs
{
    public class GuidOrNumber
    {
        #region FIELDS/CONSTANTS
        private const string GUID_TYPE = "System.Guid";
        private const string INT_TYPE = "System.Int32";
        private const string LONG_TYPE = "System.Int64";

        private Guid? _backingGuid;
        private int? _backingInt;
        private long? _backingLong;
        private ValueType _backingObject;
        private Type _underType;

        #endregion

        #region PROPERTIES
        internal bool IsGuid => _backingGuid.HasValue;
        internal bool IsInt => _backingInt.HasValue;
        internal bool IsLong => _backingLong.HasValue;

        public ValueType Value => _backingObject;

        #endregion

        #region CONSTRUCTORS
        private GuidOrNumber(Guid guid)
        {
            _backingGuid = guid;
            _backingObject = guid;
            _underType = typeof(Guid);
        }
        private GuidOrNumber(int int32)
        {
            _backingInt = int32;
            _backingObject = int32;
            _underType = typeof(int);
        }
        private GuidOrNumber(long int64)
        {
            _backingLong = int64;
            _backingObject = int64;
            _underType = typeof(long);
        }

        #endregion

        #region METHODS
        public string AsString() => Convert.ToString(_backingObject);
        public override bool Equals(object obj) => _backingObject.Equals(obj);
        public override int GetHashCode() => _backingObject.GetHashCode();
        public Type GetUnderlyingType() => _underType;
        public void SetValue(Guid guid)
        {
            _backingObject = guid;
            _backingGuid = guid;
            if (this.IsInt)
                _backingInt = null;

            else if (this.IsLong)
                _backingLong = null;
        }
        public void SetValue(int int32)
        {
            _backingInt = int32;
            if (this.IsGuid)
                _backingGuid = null;

            else if (this.IsLong)
                _backingLong = null;
        }
        public void SetValue(long int64)
        {
            _backingLong = int64;
            if (this.IsGuid)
                _backingGuid = null;

            else if (this.IsInt)
                _backingInt = null;
        }

        #endregion

        #region OPERATORS/CASTS
        public static implicit operator GuidOrNumber(Guid guid) => new GuidOrNumber(guid);
        public static implicit operator GuidOrNumber(int int32) => new GuidOrNumber(int32);
        public static implicit operator GuidOrNumber(long int64) => new GuidOrNumber(int64);
        public static explicit operator Guid(GuidOrNumber gon) => gon.IsGuid
            ? gon._backingGuid.Value
            : Guid.Empty;

        public static explicit operator int(GuidOrNumber gon) => gon.IsInt
            ? gon._backingInt.Value
            : int.MinValue;

        public static explicit operator long(GuidOrNumber gon) => gon.IsLong
            ? gon._backingLong.Value
            : long.MinValue;

        #endregion
    }
}