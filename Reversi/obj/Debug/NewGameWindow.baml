   M S B A M L   `   `   `  ����B  >Reversi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ReversiReversi  5   	       Reversi.NewGameWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Reversi     5   	     � Title$  Reversi - New Game��5   	   $	��220��6$   $	��300��61    � WindowStartupLocation$ CenterScreen=�5	   	    � 
ResizeMode$ NoResize=�6.   .��5      ��  �� Orientation$ Vertical=�6   $��5q�6(   .+�5   
   �� $��
Your name:��6   6'   �� 5   
   -   5        $��PlayerOneTextBox��5      $	��5 0q�6*   69   �� 5   
   $��	Opponent:��6   6&   � 5   
    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $��* �6#   .+�5      �� -   5        $��
AICheckBox��5      $��AI��6)   $��0��66   $��Center=�6F    d� 	IsChecked$ FalseY�6c   6�   �� 5      -   5        $��PlayerCheckBox��5      $��Player��6-   $��1��6>   $��Center=�6N   $ FalseY�6k   6�   5      �� 5   
   $��Player #2 name��6   6+   �� 5   
   -   5        $��PlayerTwoTextBox��5      {�.  
�������$	��5 0q�6R   6a   �� 5   
   -   5        $��New Game��5      $	��100��6$   $��10q�60   6Z   5      5      