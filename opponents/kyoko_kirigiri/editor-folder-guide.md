Guide to dialogue folders for Kyoko Kirigiri.

Last updated: May 2024, by Kiki Retzorg

This guide is intended for anyone writing Kyoko after the original writer.
Please think very carefully before adding a new folder at the top level. Add new sub-folders as you please, but please make sure to document new folders in here for the benefit of any subsequent writers.

# 1. ANTICS
- Silly lines not specific to a tag filter or the April Fool's event.
- Also included: lines part of a particular sequence/group not applicable to any other folder; these go in subfolders.

## Bluffs
- Lines related to Kyoko actively trying to disguise her hand strength.

## Collectible worn
- Lines while human is wearing one of Kyoko's collectibles.

## Player exposed
- Selected lines, human starts the game not wearing enough clothes.

## Show me your back
- The standard way for Kyoko to show off her butt pose.

## Underdog mode
- Kyoko loses eight times in a row, the first eight rounds of the game. This causes her AI to shoot up to Best instantly.

# 2. CLOTHING TYPE
- All lines in this folder should be in a subfolder.
- Chiefly for use on the Opponent Stripping/Stripped cases, **not** the cases that specify gender and clothing type. Specific can be an exception.
- Filtered lines take precedence over this folder.

## Extra

## Extra/Minor

### Odd order
- Weird stripping order.

## Important

## Major

## Major/Important

## Minor

## Specific
- Specific items of clothing, using ~clothing.generic~ or ~clothing~ or the like.

# 3. COSTUME
- Lines for a specific alternate costume (not default outfit).
- Each costume uses its own subfolder.
- Remember, costumes should have dedicated stripping lines for almost every stage. Be diligent about lines for the default costume that don't make sense for alternates.
- All lines in this folder should be in subfolders.

## Suit
- Future Foundation suit.

# 4. FILTERED
- Mainly **filtered lines** with a specific tag as their condition, but some additional cases and exceptions apply.
- Also included: lines based on a specific background.
- Also included: lines using the ~attracted(self)~ or ~compatible(self)~ variable tests to check for sexual orientations **not** attracted to/compatible with Kyoko.
- Lines in this folder need not actively have the filter/background condition on them,
- Targeted lines to multiple characters intended to filter for a tag that doesn't exist, e.g. the line commenting on an opponent having black underwear.
- Targeted folder takes precedence over this folder -- except for the above category.
- Prompts folder takes precedence over this folder.
- For lines filtering for tags that are seasonal or likely seasonal (e.g. Halloween Alt Skin, Santa Hat), Seasonal folder takes precedence over this folder.
- Generally, lines filtering for the basic crotch/chest size tags are **not** included, particularly on the crotch/chest reveal cases (e.g. lines for multiple small males).
- Generally, lines filtering for specifically the sexual orientations that **are** attracted to/compatible with Kyoko are **not** included.

## Advanced gender
- Support for characters with a gender/sex alignment outside the traditional understanding the game operated on, outside Mettaton and Sanny & Tess, up until futanari implementation began.

## Background
- Conditional upon background variables.
- Lines that can play on the Inventory background are **not** included.

## Danganronpa
- Filters for the Danganronpa tags.
- Filtered lines mentioning/specifically alluding to Danganronpa characters.
- Filters for the Zero Escape tag.

## Unattracted
- Filters for a character not being attraced to/compatible with Kyoko.
- Includes both tag filters and and the ~attracted(self)~ and ~compatible(self)~ variable tests. Using the variable tests instead is encouraged as much as possible; be concise with your conditions whenever you can.

# 5. FORFEIT ACTS
- Specific sequences dividing forfeit lines based on how far-along she is.
- All lines in this folder should be in Stage 9.
- Heavy lines are **not** included.
- The act number is controlled by Technical lines.

## Act 1

## Act 2

## Act 3

# 6. MS Generic Sets
- Must Strip Generic Sets.
- Lines for Opponent Lost (target layers>0), Must Strip (Male), and Must Strip (female) based on her sequence of how many times one of these triggers has happened.

## Set 1
- stripCount: 1-6.

## Set 2
- stripCount: 7-12.

## Set 3
- stripCount: 13-18.

## Set 4
- stripCount: 19-24.

## Set 5
- stripCount: 25+.

# 7. MUST FORFEIT
- Opponent Lost lines playing when the target has 0 remaining layers.
- For easier organization, standard lines for the Must Forfeit (Male) and Must Forfeit (Female) case are also included.
    - Bug text lines are **not** included. Filtered lines take precedence.

# 8. PROMPTS
- Generic or filtered lines with a prompt.
- Each of these should have a situation callout. Combining several prompts together as a 'faked' (priority=-405) situation callout for easier use is suggested.

# 9. SEASONAL
- Lines based on game conditions or tag filters specific to a certain time of year, e.g. December and birthday lines.

## April Fool's
- Non-targeted lines that, on spnati.net, can only play during the April Fool's event.
- Targeted lines toward some April Fool's characters. Generally, the more serious entries go in Targeted instead.
    - Constanze targets would go in Targeted, but AE86 go here.

# 10. SPONSORSHIPS
- Lines filling requirements for specific sponsorships.
- Subfolders are not explained here. See sponsorship.md for additional documentation.
- All lines in this folder must be in subfolders, and one sponsorship has all of its lines in further subfolders.
- This folder is likely to be dissolved and have lines reassigned as necessary after Kyoko passes QA.
- If a line is niche enough, it will not be included. If you are a Dev Mod doing QA, use sponsorship.md; the actual folders are only a supplement to it.

# 11. TARGETED
- Every non-technical line towards a specific character, with very few exceptions.
- Only Technical folder takes precedence.
- All lines should be in subfolders.

## Outbounds
- Targets toward another character.
- Stage subfolders refers to the target's stage.

### Forfeit
- All forfeit-related cases, including Must Forfeit and Finished.

### Hand
- Hand lines not part of the stripping outbounds.

### Intro/Ending
- Cases before the first strip of the game, i.e. before/as the game starts or during the first hand.
- Game Over cases.

### Stage (0-7)
- Stripping outbounds.

### Stage X
- Can play at multiple different stages.

## Replies
- Also known as Inbounds. These typically respond to a target from another character to Kyoko.
- Has many of the same subfolders as Outbounds, so not all are documented here.

### Generic
- Replies to characters' generic or filtered lines which are not prompt replies. Not necessarily Hand lines.

### Prompted
- Prompt replies, generally based on situations called out as prompts.

### Stage (0-8, X)
- Stage subfolders refer to Kyoko's stage.

### Stage X+1
- Lines that will play a stage after Stage X.

### Trio+
- Lines that depend on a combination of two or more opponents (thus, a Trio or more when including Kyoko).
- Some further subfolders are dependent on a specific trio. They are not documented. Add these as needed; generally, three or four lines is the point when you should make one of these.

#### Quartet
- Involves three specific opponents, thus requiring an entire specific table.

### Xtra
- Special cases. All should be in further subfolders.

#### Ferris pseudo-generics
- Low-priority clones of generic lines including markers that can have Kyoko call Ferris Argyle by randomly-selected pronouns in the right situation.

#### Toshinou friendship
- A group of lines based on Kyoukos Kirigiri and Toshinou replying to each other's generic lines or generally goofing off between strips.

# 12. TECHNICAL
- Lines controlling under-the-hood functions like her AI level and markers acting as variables. Most are hidden lines, but not all Technical lines are hidden and not all hidden lines are technical.

## AI ramp-up
- Standard AI level increase.

## Arousal
- Lines triggering her visible arousal markers.
- Non-hidden lines specifically commenting on her own arousal are not included.

## Boss mode
- The triggers, lines, and AI increases specific to her harder mode.

## Clothing vocabulary
- Custom terms for clothing.

### Player
- Clothing vocabulary lines for the human player that play on the stripped trigger instead of stripping.

## Collectible
- Lines that unlock collectibles or set up for the lines that unlock collectibles.
- Lines that comment on the collectible being worn belong over in Antics>Collectible worn.

## Duplicate dialogue
- Chiefly, targeted lines identical to other targeted lines for the purposes of Kyoko being able to delay her targeted dialogue if replying to a character on her right.
- Also included: Fallback lines, placed in negative priority and set to be able to play when Kyoko has exhausted her play-once lines for the case.
- Also included: Any other line deliberately identical to another line she says, intended to play in a different, mutually exclusive situation, at your discretion.
- If a line is niche enough (e.g. a filtered) and it's easier to keep both it and its duplicate in the same folder, it need not be in here. Unless it's targeted.
- Takes precedence over Targeted folder.

## Hair
- Lines controlling the ``hair`` marker.
- Lines related to the ``movedHair`` marker.
- These are two entirely different things, but they get to room together, because why not.

## Nicknames
- Hidden lines with nickname operations.

## No swap
- Hand lines extending her Swapping cards line if she doesn't swap any cards.

## Timeline
- Hidden lines controlling the ``timeline`` and ``timeline2`` marker.
