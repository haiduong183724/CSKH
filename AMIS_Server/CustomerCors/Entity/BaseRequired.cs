using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerCors.Entity
{
    [AttributeUsage(AttributeTargets.Property)]
    class BaseRequired : Attribute
    {

    }
    public class BaseNoDB : Attribute
    {

    }
    public class BaseEntityCode : Attribute
    {

    }
    public class BaseEntityId : Attribute
    {

    }
    public class BaseMaxLength : Attribute
    {
        public int maxlength;
        public BaseMaxLength(int length)
        {
            this.maxlength = length;
        }

    }
    public class MISAEmail : Attribute
    {

    }
    public class MISANumber : Attribute
    {

    }
}