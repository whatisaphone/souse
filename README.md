# Souse

Souse is a Windows app that lets you perform input actions by making audible gestures (clicks, whistles, etc.) into a mic or a headset.

At the moment, input sounds and output actions are hardcoded, however they can be adjusted in the code easily if you find code easy. Gesture discrimination happens in `AudioAnalyzer.cs`, and translation of gestures to actions happens in `App.cs`.