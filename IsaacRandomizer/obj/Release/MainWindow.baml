   M S B A M L   `   `   `  ���� J  FIsaacRandomizer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:IsaacRandomizerIsaacRandomizer       IsaacRandomizer.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     J FWpfAnimatedGif, Version=1.4.14.0, Culture=neutral, PublicKeyToken=null,gif"http://wpfanimatedgif.codeplex.com  )localclr-namespace:IsaacRandomizer  -     � Title$  Isaac Randomizer��$��686.516��$��811.666�� � 
ResizeMode$ CanMinimize=�.��� $��0,0,0,1q� � ColumnDefinitions �� $
��283* ��� $
��239* �.+��� -    ��Name$ 
tabControl��$
��Left=�$	��437��$��	0,110,0,0q�$	��Top=�$	��806��$��2�� � 	Resources  �� 
TargetType �� Triggers P� Property P� Value	 �� Property
 �� Value��%l   '��       �� " �� P�  �@��
IsSelected . . ��� 	 � ��
 �� .R��� $
��Bold"����� .@��� $��Presets��$
��Left=�$	��Top=�$	��268��.��� ���	#FFE5E5E5.+��� -    ��Name$ 
loadPreset��$��Load Preset��$��Center=�$��269,44,429,0q�$	��Top=�$��71���� -   $ 
savePreset��$��Save Preset��$��Center=�$��269,20,429,0q�$	��Top=�$��71�� J� RenderTransformOrigin$ -0.305,0.517&��� -    ��Name$ 
presetList��$��Center=�$	��329��$��0,69,0,0q�$	��Top=�$	��231���� -    ��Name$ 
presetName��$��Center=�$��20��$��345,19,269,0q� �� TextWrapping$ NoWrap=�$	��Top=�$	��155��$��Preset1�� �� MaxLines$ 1���� -   $ deletePreset��$��Delete Preset��$��Center=�$��425,44,269,0q�$	��Top=�$��75���� -   $ updatePreset��$��Update��$��Center=�$��345,44,349,0q�$	��Top=�$��75���� -	    ��Name$ image2��$
��Left=�$	��258��$��57,0,0,0q�$��Center=�$	��169��$��doodle3.png�� �� BitmapScalingMode$ NearestNeighbor=�$��Uniform=��� -
   $ image2_Copy��$
��Left=�$	��258��$��563,75,0,75q�$��Center=�$	��169��$��doodle4.png��$ NearestNeighbor=�$��Uniform=��� $��Graphics & Sound��$
��Left=�$	��266��.��� ���	#FFE5E5E5$��0,0,-1,0q�$
��Left=�$	��801��.+��� -   $ image��$��Center=�$	��134��$��271,265,271,0q�$	��Top=�$	��246��$��doodle1.png��$ NearestNeighbor=�$ 0.5,0.5&�$��Uniform=��� -    ��Name$ doesRandomizeMusic��$��Randomize Music!��$
��Left=�$��275,139,0,0q�$	��Top=��� -   $ doesRandomizeSounds��$��Randomize Sounds��$
��Left=�$��
275,70,0,0q�$	��Top=��� -    ��Name$ label3��$��Randomizer Intensity��$
��Left=�$��
275,85,0,0q�$	��Top=��� -   $ textBox1��$
��Left=�$��23��$��277,111,0,0q�$
 Wrap=�$��4��$	��Top=�$	��120��{�.   �� 	MaxLength$ 2���� -   $ doesRandomizeCurses��$��Randomize Curse Names��$
��Left=�$��
275,30,0,0q�$	��Top=�$	��162���� -   $ 	checkBox2��$��Randomize Stages��$
��Left=�$��
275,10,0,0q�$	��Top=�$	��151���� -   $ originalWords��$��Classic Words��$
��Left=�$��
10,130,0,0q�$	��Top=�{�.  �� -   $ 	edgyWords��$��	New Words��$
��Left=�$��
10,150,0,0q�$	��Top=�{�.  $	��122���� -   $ 	jokeWords��$��NSFW and Joke Words��$
��Left=�$��
10,170,0,0q�$	��Top=�{�.  �� -   $ doesRandomizeBossNames��$��Randomize Boss Names��$
��Left=�$��	10,70,0,0q�$	��Top=�	� -    	�Name$ bossNameConsistent��$��Relevant��$
��Left=�$��	10,90,0,0q�$	��Top=�{�.  	� -   $ bossNameRandom��$��
Irrelevant��$
��Left=�$��
10,110,0,0q�$	��Top=�{�.  �� -   $! doesRandomizeCharacterNames��$��Randomize Player Names��$
��Left=�$��	10,10,0,0q�$	��Top=��� -   $# doesRandomizeCharacterSprites��$��Randomize Player Sprites��$
��Left=�$��	10,30,0,0q�$	��Top=��� -   $ 	checkBox9��$��Randomize Cards��$
��Left=�$��544,150,0,0q�$	��Top=��� -   $ 
checkBox10��$��Randomize Pills��$
��Left=�$��544,170,0,0q�$	��Top=�$ 0.296,-1&��� -   $ checkBox��$��Randomize Items��$
��Left=�$��
543,30,0,0q�$	��Top=��� -   $ 	checkBox3��$��Randomize Item Sprites��$
��Left=�$��
543,10,0,0q�$	��Top=�$	��165��� $
��Left=�$��40��$��
544,50,0,0q�$	��Top=�$	��114��.+�	� -   $ radioButton��$��Relevant Names��$
��Left=�$��0,3,0,0q�$	��Top=�{�.  	� -   $ radioButton1��$��Irrelevant Names��$
��Left=�$��0,23,0,0q�$	��Top=�{�.  $ 1.009,3.067&�� $
��Left=�$��61��$��
10,220,0,0q�$	��Top=�$	��125��.+�	� -    $ 	fontIsaac��$��
Isaac Font��$
��Left=�$	��Top=� d� 	IsChecked$
 TrueY�	� -!   $ fontReadable��$��Alternate Font��$
��Left=�$	��Top=�$��0,20,0,0q��� -"   $ removePauseMenuIcons��$"��Remove Item Pause Menu Icons��$
��Left=�$��544,110,0,0q�$	��Top=��� $��Gameplay��$��Right=�$	��268��.��� ���	#FFE5E5E5.+��� -#   $ 	doesMaxHp��$!��Randomize Max HP Multiplier��$
��Left=�$��
532,42,0,0q�$	��Top=��� -$   $ maxHp��$
��Left=�$��15��$��
739,42,0,0q�$
 Wrap=�$	��Top=�$��50��{�.  $��1���� -%   $ 
doesHpGain��$!��Randomize Health Multiplier��$
��Left=�$��
532,62,0,0q�$	��Top=��� -&   $ hpGain��$
��Left=�$��15��$��
739,62,0,0q�$
 Wrap=�$	��Top=�$��50��{�.  $��1���� -'   $ doesSoulGain��$%��Randomize Soul Heart Multiplier��$
��Left=�$��
532,82,0,0q�$	��Top=��� -(   $ soulGain��$
��Left=�$��15��$��
739,82,0,0q�$
 Wrap=�$	��Top=�$��50��{�.  $��1���� -)   $ doesBlackGain��$&�� Randomize Black Heart Multiplier��$
��Left=�$��532,102,0,0q�$	��Top=��� -*   $ 	blackGain��$
��Left=�$��15��$��739,102,0,0q�$
 Wrap=�$	��Top=�$��50��{�.  $��1���� -+   $ doesFullGain��$"��Randomize Full HP Multiplier��$
��Left=�$��532,122,0,0q�$	��Top=��� -,   $ fullGain��$
��Left=�$��15��$��739,122,0,0q�$
 Wrap=�$	��Top=�$��50��{�.  $��1���� --   $ doesBombGain��$��Randomize Bomb Multiplier��$
��Left=�$��532,142,0,0q�$	��Top=��� -.   $ bombGain��$
��Left=�$��15��$��739,142,0,0q�$
 Wrap=�$	��Top=�$��50��{�.  $��1���� -/   $ doesKeyGain��$��Randomize Key Multiplier��$
��Left=�$��532,162,0,0q�$	��Top=��� -0   $ keyGain��$
��Left=�$��15��$��739,162,0,0q�$
 Wrap=�$	��Top=�$��50��{�.  $��1���� -1   $ doesCoinGain��$��Randomize Coin Multiplier��$
��Left=�$��532,182,0,0q�$	��Top=�$ 2.23,13.067&��� -2   $ coinGain��$
��Left=�$��15��$��739,182,0,0q�$
 Wrap=�$	��Top=�$��50��{�.  $��1���� -3   $ 	checkBox7��$��Randomize Active Charges��$
��Left=�$��263,144,0,0q�$	��Top=�� $
��Left=�$��49��$��291,164,0,0q�$	��Top=�$	��161��.+�	� -4   $ activeChargesFair��$
��Fair��$
��Left=�$	��Top=�{�.  	� -5   $ activeChargesRandom��$��Random��$
��Left=�$	��Top=�{�.  $��0,20,0,0q��� -6   $ 	checkBox5��$��Disable Special Flag��$
��Left=�$��263,124,0,0q�$	��Top=��� -7   $ 	checkBox6��$��Randomize Item Pools��$
��Left=�$��
263,41,0,0q�$	��Top=��� -8   $ doesRandomizeRarities��$��Randomize Item Rarities��$
��Left=�$��
263,61,0,0q�$	��Top=�$	��164��� $��291,81,328,289q�.+�	� -9   $ raritiesFair��$
��Fair��$
��Left=�$	��Top=�{�.  	� -:   $ raritiesRandom��$��Random��$
��Left=�$	��Top=�$��0,20,0,0q�{�.  �� -;   $  doesRandomizePlayerPickups��$��Randomize Player Pickups��$
��Left=�$��
10,104,0,0q�$	��Top=�� $
��Left=�$	��146��$��
39,124,0,0q�$	��Top=�$	��142��.+�	� -<   $ hasSpecificAmountPickups��$��Pickups Each��$	��Top=�{�.  $��0,0,22,0q��� -=   $ pickupsEach��$��16��$��103,0,-11,0q�$��50��$
 Wrap=�$��3��$	��Top=�{�.  $ 	0.392,0.5&�	� ->   $ hasTotalAmountPickups��$��Total Amount��$��	0,20,42,0q�$��15��$	��Top=�{�.  �� -?   $ totalPickup��$��16��$��103,18,-11,0q�$��50��$
 Wrap=�$��8��$	��Top=�{�.  �� -@   $ coinMultiplier��$��
More Coins��$��Right=�$��	0,60,22,0q�$	��Top=�{�.  $	��120��	� -A   $ radioButton2��$��Random��$
��Left=�$��0,40,0,0q�$	��Top=�{�.  �� -B   $ doesRandomizePlayerItems��$��Randomize Player Items��$
��Left=�$��	10,41,0,0q�$	��Top=��� -C   $ specificItemAmount��$
��Left=�$��16��$��
121,61,0,0q�$
 Wrap=�$��2��$	��Top=�{�.  $��50���� -D   $ hasSpecificAmountItems��$��
Items Each��$
��Left=�$��	39,61,0,0q�$	��Top=�{�.  �� -E   $ ensureActiveItem��$��Ensure Active Item��$
��Left=�$��	39,84,0,0q�$	��Top=�{�.  �� -F   $ label9��$��Players��$
��Left=�$��	10,10,0,0q�$	��Top=�$
��Bold"��� -G   $ label7��$��Item Basics��$
��Left=�$��
263,10,0,0q�$	��Top=�$
��Bold"��� -H   $ label8��$��Item Pickups��$
��Left=�$��
532,11,0,0q�$	��Top=�$ 0.474,-1.192&�$
��Bold"��� -I   $ image1��$��Center=�$��274,251,274,0q�$	��Top=�$��doodle2.png��$ NearestNeighbor=�$ 0.5,0.5&�$��Uniform=�$	��148��$	��252���� -J   $ textBox��$
��Left=�$��20��$��
10,625,0,0q�$
 Wrap=�$	��Top=�$	��786�� � ToolTip$T NEnter your The Binding of Isaac: Afterbirth Steam installation directory here.��$��2���� -K   $ button��$��
RANDOMIZE!��$��	0,552,0,0q�$��Center=�$��2��$��50(�   IsaacHandwritten#��[  $	��233��$��39��$	��Top=�$ 0.5,0.5&�z�V� .~��� ��  �� AngleY$ -0.131Q��� T�  T� Y$ -0.331Q� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     
BlurRadius$ 3Q�   ShadowDepth$ 5Q��� -L   $ deletesCurrentFiles��$
 TrueY�$%��Delete Current Randomizer Files��$
��Left=�$��169,552,0,0q�$	��Top=�$��1��
��������� -M   $ overwriteOriginalFiles��$
 TrueY�
�������$"��Overwrite Current Isaac Mods��$
��Left=�$��182,572,0,0q�$	��Top=�$��1���� -N   $ label��$��
Isaac Path��$��Center=�
�������$��374,599,348,0q�$	��Top=�$��2��$��84���� -O   $ image3��"  WpfAnimatedGif.ImageBehavior   AnimatedSource$ logo.gif��$
��Left=�$	��110��$	��Top=�$	��806��$��2��$��UniformToFill=��� -P   $ image4��$ backdrop.gif��$
��Left=�$	��110��$	��Top=�$	��806��$��2��$��UniformToFill=�$��	0,547,0,0q� K� ZIndex$ -1���� -Q   $ button1��$��Remove Randomizations��$��1��$
��Left=�$��210,592,0,0q�$	��Top=�$	��149���� -R   $ music��$��Music!��$
��Left=�$��
10,552,0,0q�$	��Top=�
�������r� -S     r�Name$  Media��$��114,534,261,54q�$ 0.5,0.5&�$y�Hidden=�