   M S B A M L   `   `   `  ����B  >Reversi, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ReversiReversi  5   	       Reversi.HighscoresWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:Reversi     5   	     � Title$  Reversi - High scores��5   	   $	��700��6'   $	��550��64    � WindowStartupLocation$ CenterScreen=�5	   	    � 
ResizeMode$ NoResize=�6.   .��5
      �  � RowDefinitions 5   
   �� 5      $	��16* �6   �� 5      $��* �6   .+�5   
   �� -   5        $��ListView��5      $��0��6#   $��10q�60   ��6<   ��  �� View 5      �� 5      .P�5      �� $��White Player��6%    �� Width$	 100��6;    �� DisplayMemberBinding 6G   ��  �� Path$ WhitePlayer��� 5      $��Black Player��6%   $	 100��6;    6G   �� $ BlackPlayer��� 5      $��White Points��6%   $ 75��6;    6F   �� $ BlackPoints��� 5      $��Black Points��6%   $ 75��6;    6F   �� $ BlackPoints��� 5      $
��Time��6%   $ 70��63    6>   �� $
 Time��� 5      $
��Date��6%   $ 80��63    6>   �� $
 Date�5      5      �� 5   
   -   5        $��ClearButton��5      $��1��6$   $��8q�62   $��Clear High Scores��6=   
��� E��
�������6�   5      5      