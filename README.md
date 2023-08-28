# Passwords

The app generates N random sequences (Total sequences) of the predefined length (Length of sequences) with the additional randomization complexity (Complexity) and saves them into a file (File). 

Complexity

Since the randomization algorythm is not completely random, additional step generates first a list with the number of items = (Length of sequences) * (Complexity) and then uses it to randomly select items for the final string.

Sequences 

The app uses lists to generate random sequences of symbols:
- Latin letters
- Numbers
- Extra symbols
- German letters
- Currencies

The lists can be turned on/off in “Settings”.

Dictionary

The app uses a text file (Dictionary.txt) with symbols/words/phrases created by the user and a separator symbol (Separator, “Settings”, can be set to empty). The dictionary file is located in the same folder with the app.

Manual

1. Number, length of sequences and complexity can be typed into the text boxes or set using the mouse wheel.

2. Result file name (File) is generated automatically indicating the length of sequences, but can be changed through typing a name into the text box. 

3. Target directory (“Settings”) defines the location of the result file on your computer. It can be set through typing or using the mouse wheel (upon the start of the application, the list of folders in the root directory of the app - location drive is generated). When set, the target directory is automatically added to the result file name.

Good luck!
