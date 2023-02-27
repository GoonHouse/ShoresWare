using ShoresWare.Core;
using UnityEngine;

namespace ShoresWare.Games._DEBUG.DBG_COMMAND.Scripts {
    public class CommandSpam : MicroGameObject
    {
        public override void OnMicroGameStart(int lengthInBeats, Data.GameDifficulty difficulty) {
            base.OnMicroGameStart(lengthInBeats, difficulty);
            var boom = FindObjectOfType<BoomBox>();
            for (int i = 1; i <= lengthInBeats; i++) {
                boom.ScheduleOnRelativeBeat(i, int0 => {
                    var elev = FindObjectOfType<Elevator>();
                    var text = $"beat {int0}/{lengthInBeats}";
                    Debug.Log(text);
                    elev.SetCommand(text);
                });                
            }
            
        }
    }
}
