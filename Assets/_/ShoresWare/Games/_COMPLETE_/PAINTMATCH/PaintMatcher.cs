using System;
using ShoresWare.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace ShoresWare.Games.PAINTMATCH {
    public class PaintMatcher : MicroGameObject {
        public Slider red;
        public Slider green;
        public Slider blue;

        public Image colorTarget;
        public Image colorGoal;

        public Vector3 goal;

        private int steps;

        public override void OnMicroGameStart(int lengthInBeats, Data.GameDifficulty difficulty) {
            base.OnMicroGameStart(lengthInBeats, difficulty);
            switch (difficulty) {
                case Data.GameDifficulty.GAME_DIFFICULTY_NONE:
                    break;
                case Data.GameDifficulty.GAME_DIFFICULTY_EASY:
                    steps = 3;
                    break;
                case Data.GameDifficulty.GAME_DIFFICULTY_MEDIUM:
                    steps = 4;
                    break;
                case Data.GameDifficulty.GAME_DIFFICULTY_HARD:
                    steps = 6;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
            }

            red.maxValue = steps;
            green.maxValue = steps;
            blue.maxValue = steps;

            goal = new Vector3(
                Random.Range(0, steps),
                Random.Range(0, steps),
                Random.Range(0, steps)
            );

            colorGoal.color = new Color(
                BeeCore.MathBee.Scale(goal.x, 0, steps, 0.0f, 1.0f),
                BeeCore.MathBee.Scale(goal.y, 0, steps, 0.0f, 1.0f),
                BeeCore.MathBee.Scale(goal.z, 0, steps, 0.0f, 1.0f)
            );
        }

        // Update is called once per frame
        void Update() {
//            colorTarget.color = new Color(red.normalizedValue, green.normalizedValue, blue.normalizedValue);
            colorTarget.color = new Color(
                BeeCore.MathBee.Scale(red.value, 0, steps, 0.0f, 1.0f),
                BeeCore.MathBee.Scale(green.value, 0, steps, 0.0f, 1.0f),
                BeeCore.MathBee.Scale(blue.value, 0, steps, 0.0f, 1.0f)
            );
            
            if (Mathf.Approximately(red.value, goal.x) &&
                Mathf.Approximately(green.value, goal.y) &&
                Mathf.Approximately(blue.value, goal.z)) {
                Win();
            }
        }
    }
}
