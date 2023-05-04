using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.InterfaceCollection
{
    public interface IHealth
    {
        int LifePoints { get; set; }
        int MaxLifePoints { get; }

        int AddDamage(int attackValue, int ignoringValue);

        



    }
}
