   M S B A M L   `   `   `  ����@  <dubao, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:dubaodubao  5   	       dubao.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   %localclr-namespace:dubao     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5	      �  � RowDefinitions 5
   
   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   6.   �� 5      $��* �6   6+   .+�5   
   ]� $��0��6   $��0��6!   $
��Left=�61   .+�5      �� $��Nhập số��6   6(   �� 5      -   5         ��Name$ 	txtnhapso��5      $	��120��6N   
���  ��$��16(�6k   ��6)   �� *Songay+ �� Mode$ TwoWay=�6{   5      ]� 5   
   $��0��6   $��1��6"   .+�5      �� -   5        $
��Chay��5      $��70��6$   $
��Left=�6/   $y�Hidden=�6�   ��6_   �� *Text+ �� ElementName$ 	txtnhapso��6�   �� 5      -   5        $��AI Dự đoán��5      $��70��6*   $
��Left=�65   6i   �� 5      
������$��18(�6f   $��Arial)�6t   $
��Bold"�6�   $��Italic$�6�   ��6   �� *SelectedItem.Name+$ cobdai��6�   5      � 5   
   $��2��6   $��2��6    5      �� 5      $
��Auto �6    60   �� 5      $��* �6    6-    5       �� 5!      $
��Auto �6#   62   �� 5"      $��* �6#   6/   .+�5$      ]� $��0��6   $��0��6%   $��2��65   .+�5%      �� $3��-Xác Suất tối thiểu đánh trúng là: ��6   6G   �� 5&      
���  ��$��16(�6E   $
��Bold"�6S   ��6   �� *	xstrung+6g   5'      �� 53      $��1��6   $��0��6(   ��68   �� *listcolection+ �� View 5?      �� 5@      .P�5A      �� $
��Số��6-    �� DisplayMemberBinding 69   �� *num+6`   �� 5B      $��
Số lần��6-    6=   �� *count+6f   �� 5C      $��
xac suất��6-    6?   �� *xs+6e   5D      5F      �� 5H      $��1��6>   $��1��6K   ��6   �� *ListPrediction+ 5I      �� 5J      .P�5K      �� $
��Số��6)    65   �� *Bi+6[   �� 5L      $��Dự đoán��6)    6:   �� *P+6_   5M      5O      5R      �� 5T   
   $��1��6   $��0��6"   .+�5U      �� -   5         ��Name$ cobdai��5U   7   	 �� SelectedValuePath$	 Id��6G   ��6   �� *	listDai+��5V      �� 5W      .R�5X      �  5\      �� 5]   "   $
��Auto �63   6B   �� 5^   "   $
��Auto �63   6B   �� 5_   "   $��* �63   6?   .+�5a      �� $��0��6&   ��66   �� *Id+6L   �� 5b      $��1��6(   ��68   �� *Name+6P   �� 5c      $��2��6&   ��66   �� *Url+6M   5d      5e      ��5g      �� 5h      .R�5i      $� 
 K� IsItemsHost$

 True��61    $� Orientation$ Vertical=�6D   6]   5j      5l      �� 5m      -   5        $��Get data��5m      6A   5n      5o      5p      