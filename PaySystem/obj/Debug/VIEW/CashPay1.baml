   M S B A M L   `   `   `  ����D  @PaySystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:PaySystem.VIEWPaySystem.VIEW  5   	       PaySystem.VIEW.CashPay1   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:PaySystem.VIEW     5   	     � Title$  CashPay1��5   	   $
��1024��6   $
��1280��6(    � WindowStyle$
 None=�65    � WindowStartupLocation$ CenterScreen=�6H    � WindowState$ 	Maximized=�6m   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $
��105* �6   �� 5      $
��108* �6   �� 5      $
��105* �6    � RowDefinitions 5   
   �� 5      $	��88* �6   �� 5      $	��39* �6   .+�5   
   � $	��200q�6   $��3��6   ���	#FF2F3466.+�5      �� -   5         ��Name$ label��5      $��车牌: 粤B5YY49��6#   $��
81,35,81,0q�69   
�������$��57��6`   $	��Top=�6l   $��36(�6�   �� 5      -   5        $ labe2��5      $'��!应付金额：               元��6#   $��
81,97,81,0q�6C   $��36(�6W   
�������$��57��6x   $	��Top=�6�   �� 5      -   5        $ labe3��5      $��57.00��6#   $��260,97,258,0q�63   ���	#FFE4C70F$��40(�6`   $��57��6n   $	��Top=�6z   �� 5      -   5         ��Name$ prompt��5      $
��Wrap=�6(   $��36(�6<   $��81,159,81,10q�6J   $�� ��6`   $	��125��6i   ���	#FFFF440C6v   5      �� 5   
   -   5         ��Name$ button��5      $��返回首页��6!   $��82��60   $��
57,54,25,0q�6<   $��1��6P   $	��Top=�6]   
�������$��36(�6�   ��6�   Z� 6�   �� 5   
   -   5        $ button1��5      $��现金退回��6"   ���	#FF419AFF$��1��6I   $��82��6Y   $��
25,54,25,0q�6e   $��1��6y   $	��Top=�6�   
�������$��36(�6�   ��6�   Z� 6�   �� 5   
   -   5        $ button2��5      $��确认支付��6"   ���	#FF00C17F$��2��6I   $��82��6Y   $��
25,54,57,0q�6e   $��1��6y   $	��Top=�6�   
�������$��36(�6�   ��6�   Z� 6�   5      5      