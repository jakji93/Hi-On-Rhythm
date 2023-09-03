using UnityEngine;

namespace Kamgam.SettingsGenerator
{
   [CreateAssetMenu(fileName = "SpecialEffectConnection", menuName = "SettingsGenerator/Connection/SpecialEffectConnection", order = 4)]
   public class SpecialEffectConnectionSO : BoolConnectionSO
   {
      protected SpecialEffectConnection _connection;
      public override void DestroyConnection()
      {
         if (_connection != null)
            _connection.Destroy();

         _connection = null;
      }

      public void Create()
      {
         _connection = new SpecialEffectConnection();
      }

      public override IConnection<bool> GetConnection()
      {
         if (_connection == null)
            Create();

         return _connection;
      }
   }
}

