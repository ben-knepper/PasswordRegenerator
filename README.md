# PasswordRegenerator

This is a port of my password manager from UWP to Xamarin. The app "recalls" passwords without saving them or the master password anywhere, in any form. It does this by using a hashing algorithm as a pseudo-random number generator, taking in the master password, keyword, and other components. The old version used a jury-rigged and rather naive algorithm using SHA-1. While the old hashing algorithm will remain for legacy reasons, I will be adding a new algorithm based on the Keccak sponge construction, the basis of SHA-3.

Additional features will include metadata syncing, an editable list of used keywords, and web browser integration.
