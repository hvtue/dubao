   M S B A M L   `   `   `  ����@  <dubao, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:dubaodubao  5   	       dubao.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:dubao     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   6.   �� 5      $��* �6   6+   .+�5   
   ]� $��0��6   $��0��6!   $
��Left=�61   .+�5      �� $��Nhập số��6   6(   �� 5      -   5         ��Name$ 	txtnhapso��5      $	��120��6N   ��6)   �� *nhapso+ �� Mode$ TwoWay=�6\   5      �� 5   
   -   5        $
��Chay��5      $��0��6    $��1��6-   $��70��6=   $
��Left=�6H   ��6x   �� *Text+ �� ElementName$ 	txtnhapso��6�   �� 5   
   $��2��67   $��2��6D    �� MaxLines$	 100��6T    �� MinLines$ 5��6c   ��6   �� *result+$ TwoWay=�6r   �� 5   
   $��1��6   $��0��6"   .+�5      �� -   5         ��Name$ cobdai��5   7   	 �� SelectedValuePath$	 Id��6G   ��6   �� *	listDai+��5      �� 5      .R�5      �  5       �� 5!   "   $
��Auto �63   6B   �� 5"   "   $
��Auto �63   6B   �� 5#   "   $��* �63   6?   .+�5%      �� $��0��6&   ��66   �� *Id+6L   �� 5&      $��1��6(   ��68   �� *Name+6P   �� 5'      $��2��6&   ��66   �� *Url+6M   5(      5)      ��5+      �� 5,      .R�5-      $� 
 K� IsItemsHost$

 True��61    $� Orientation$ Vertical=�6D   6]   5.      50      �� 51      -   5        $��Get data��51      6A   52      53      54      