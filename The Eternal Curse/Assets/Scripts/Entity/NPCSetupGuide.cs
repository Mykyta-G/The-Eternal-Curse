using UnityEngine;

/*
 * NPC INTERACTION SYSTEM SETUP GUIDE
 * 
 * This system allows NPCs to interact with the player using a dialog system.
 * 
 * SETUP INSTRUCTIONS:
 * 
 * 1. NPC SETUP:
 *    - Create an NPC GameObject in your scene
 *    - Add the NPCInteraction script to the NPC GameObject
 *    - Set the interaction range (default: 3f) - distance to start interaction
 *    - Set the cancel range (default: 5f) - distance where dialog cancels (must be > interaction range)
 *    - Set the interaction key (default: E)
 *    - Assign the dialog lines in the npcDialog array
 *    - Set the npcName for the dialog title
 * 
 * 2. INTERACTION INDICATOR SETUP:
 *    - Create a child GameObject under the NPC for the "!" indicator
 *    - Add the InteractionIndicator script to this child GameObject
 *    - The indicator will automatically position itself above the NPC
 *    - You can customize the appearance in the inspector
 * 
 * 3. DIALOG UI SETUP:
 *    - Create a UI Canvas with a TextMeshProUGUI component for dialog text
 *    - Create another TextMeshProUGUI component for the title (speaker name)
 *    - Add the Dialog script to the Canvas or a child GameObject
 *    - Assign the TextMeshProUGUI component to the dialog script (textMeshProUGUI)
 *    - Assign the title TextMeshProUGUI component to the dialog script (titleTextMeshProUGUI)
 *    - Set the text speed and size as desired
 * 
 * 4. PLAYER SETUP:
 *    - Make sure your player GameObject has the "Player" tag
 *    - The NPC will automatically find the player using this tag
 * 
 * 5. CONNECTING COMPONENTS:
 *    - In the NPCInteraction script, assign the interactionIndicator reference
 *    - In the NPCInteraction script, assign the dialogScript reference
 * 
 * USAGE:
 * - When the player approaches the NPC within the interaction range, a "!" will appear
 * - Press E (or the configured key) to start the dialog
 * - The dialog title will show the NPC's name
 * - Click, press Space, or press Enter to advance through dialog lines
 * - If the player moves too far away (beyond cancel range), the dialog will automatically close
 * - The dialog will automatically close when finished
 * 
 * NEW FEATURES:
 * - Cancel Range: Dialog automatically closes if player moves too far away
 * - Title System: Shows the NPC's name at the top of the dialog
 * - Visual Range Indicators: Yellow sphere (interaction) and red sphere (cancel) in Scene view
 * 
 * CUSTOMIZATION:
 * - You can modify the interaction range, cancel range, key, and dialog lines per NPC
 * - The interaction indicator can be customized with different text, colors, and animations
 * - The dialog system supports custom text speed and styling
 * - Each NPC can have a unique name displayed in the dialog title
 */

public class NPCSetupGuide : MonoBehaviour
{
    // This script is just for documentation purposes
    // You can delete it after reading the setup instructions above
    
    void Start()
    {
        Debug.Log("NPC Interaction System Setup Guide - Check the script comments for detailed instructions!");
    }
}
