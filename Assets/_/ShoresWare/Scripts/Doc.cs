namespace ShoresWare {
    public class Doc {
        /*
         * AREA: Screen Rect
         * ART: An Art Animation, With All Frames
         * OBJECT: Basic MicroGameObject
         *
         * CONTACT_TYPE: [TOUCH, OVERLAP]
         * CARDINAL: [N, NE, E, SE, S, SW, W, NW]
         * MOVE_SPEED: [Slowest, Slow, Normal, Fast, Fastest]
         * SCREEN_POINT: [SCREEN_X, SCREEN_Y]
         * ROAM_STYLE: [WIGGLE, INSECT, REFLECT, BOUNCE]
         * ROAM_SPECIFICS: [ANYWHERE, NO_OVERLAP]
         * MOVE_DESIGNATION:
         * 1) Specific direction [CARDINAL]
         * 1.1) Speed [MOVE_SPEED]
         * 2) Random direction [CARDINAL]
         * 2.1) Speed [MOVE_SPEED]
         * 3) Specific location [SCREEN_POINT]
         * 3.1) Speed [MOVE_SPEED]
         * 
         * MAKE OBJECT =>
         * 1) set art
         * 2) set position
         * 2.1) location?
         * 2.1.1) point?
         * 2.1.2) area? {rect}
         * 2.1.2.1) anywhere?
         * 2.1.2.2) try not to overlap?
         * 2.2) attachment?
         *
         * Triggers
         * 1) Tap
         * 1.1) This object
         * 1.2) Anywhere on stage
         *
         * 2) Time
         * 2.1) Exactly
         * 2.1.1) 1-1 -> 8-4 + End (33 positions)
         * 2.2) Randomly
         * 2.2.1) Start [1-1 -> 8-4 + End (33 positions)]
         * 2.2.2) End [1-1 -> 8-4 + End (33 positions)]
         *
         * 3) Contact
         * 3.1) Touch
         * 3.1.1) Another object [OBJECT]
         * 3.1.2) Location [AREA]
         * 3.2) Overlap
         * 3.2.1) Another object ???
         * 3.2.2) Location
         *
         * 4) Switch
         * 4.1) [object select]
         * 4.1.1) Switch turns ON
         * 4.1.2) Switch is ON
         * 4.1.3) Switch turns OFF
         * 4.1.4) Switch is OFF
         *
         * 5) Art
         * 5.1) Specific art ???
         * 5.2) When art finishes playing
         *
         * 6) Win/Loss
         * 6.1) Win
         * 6.2) Loss
         * 6.3) Has been won
         * 6.4) Has been lost
         * 6.5) Not yet won
         * 6.6) Not yet lost
         *
         * Action
         * 1) Travel
         * 1.1) Go straight [POINT? REL_POINT, MOVE_DESIGNATION]
         * 1.1.1) Current location [MOVE_DESIGNATION]
         * 1.1.2) Another location [((OBJECT)?,SCREEN_POINT),MOVE_DESIGNATION]
         * 1.2) Stop
         * 1.3) Jump to [POINT? REL_POINT]
         * 1.4) Swap [OBJECT]
         * 1.5) Roam [ROAM_STYLE, ROAM_SPECIFICS, AREA]
         * 1.6) Target [OBJECT, REL_POINT, MOVE_SPEED] 
         * 
         * 2) Switch
         * 2.1) Turn switch ON
         * 2.2) Turn switch OFF
         * 
         * 3) Lose (does nothing if already won)
         * 
         * 4) Art
         * 4.1) Switch art [ART]
         * 4.2) Stop art playing
         * 
         * 5) Sound Effect
         * 5.1) Action
         * 5.1.1) Explosion
         * 5.1.2) Glass
         * 5.1.3) Gong
         * 5.1.4) Spring
         * 5.1.5) Pistol
         * 5.1.6) Slice
         * 5.1.7) Camera
         * 5.1.8) Splash
         * 5.2) Computer
         * 5.2.1) Correct
         * 5.2.2) Incorrect
         * 5.2.3) Switch
         * 5.2.4) Input
         * 5.2.5) Falling
         * 5.2.6) Wiggle
         * 5.2.7) Rising
         * 5.2.8) Victory
         * 5.3) Sports
         * 5.3.1) Batting
         * 5.3.2) Swing
         * 5.3.3) Impact
         * 5.3.4) Kick
         * 5.3.5) Racquet
         * 5.3.6) Bowling
         * 5.3.7) Sunk Putt
         * 5.3.8) Whistle
         * 5.4) General
         * 5.4.1) Frying Pan
         * 5.4.2) Bell
         * 5.4.3) Knife Chop
         * 5.4.4) Cell Phone
         * 5.4.5) Shaver
         * 5.4.6) Old Phone
         * 5.4.7) Popped Cork
         * 5.4.8) Water
         * 5.5) Body
         * 5.5.1) Sneeze
         * 5.5.2) Snap
         * 5.5.3) Munching
         * 5.5.4) Gulp
         * 5.5.5) Punch
         * 5.5.6) Foot Stamp
         * 5.5.7) Gasp
         * 5.5.8) Applause
         * 5.6) Creature
         * 5.6.1) Cat
         * 5.6.2) Big Dog
         * 5.6.3) Pig
         * 5.6.4) Small Dog
         * 5.6.5) Wolf
         * 5.6.6) Crow
         * 5.6.7) Tiger
         * 5.6.8) Wing Flap
         * 5.7) Voice
         * 5.7.1) Baby
         * 5.7.2) Giggle
         * 5.7.3) Scream
         * 5.7.4) Bummed
         * 5.7.5) Kung Fu
         * 5.7.6) Let's Fight
         * 5.7.7) Cheering
         * 5.7.8) Booing
         * 5.8) 8-Bit
         * 5.8.1) Mario Jump
         * 5.8.2) Coin
         * 5.8.3) Power Up
         * 5.8.4) Power Down
         * 5.8.5) Shell Kick
         * 5.8.6) Cannon
         * 5.8.7) Struck
         * 5.8.8) Barrel Hop
         * 
         * 6) Stage Effect
         * 6.1) Flash {screen turns white for an instant}
         * 6.2) Shake {screen shakes violently, briefly}
         * 6.3) Confetti {confetti dispenses from screen top}
         * 6.4) Freeze {screen inverts, (all AI stops???), input freezes}
         */
    }
}