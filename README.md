# SketchoBOT
An AI controlled sketch guesser for the game at www.guessasketch.com

# DigitalPrinter
An earlier version of SketchoBOT that only draws images provided with a URL. The console application will convert the image to 1-bit and take control over the mouse to click in all the right places.
Certain parameters can be added after the URL to change how the image is drawn. These include:

    -down : Draws each row starting from the top.
    -up : Same as "down" except starting from the bottom.
    -rand : Draws pixels randomly.
    -curt : A curtain effect, drawing rows from both top and bottom.
    -spiral : A favorite of mine. Starts in the center and spirals outwards (End result will be slightly distorted).
    -draw : A slightly faster way of drawing. Draws 45° lines instead of each pixel. Perfect for thick lined drawings.

It's best to use line drawings instead of normal fully colored images. Thin lines will also not look good.

# Guesser
This is the main application. It will connect to the server at www.guessasketch.com with the provided username and let the user interact in the same way as the flash client does. AI guessing is available as well as manual input. The AI will "learn" from playing by remembering words other users are guessing. In other words, SketchoBOT won't look at the drawing at all. Instead it'll guess words that physically look like the words other players are guessing. The more games it plays, the better it gets at guessing.

Detailed documentation about the use of the interface will be provided if needed.
