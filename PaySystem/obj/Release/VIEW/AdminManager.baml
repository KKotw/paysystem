   M S B A M L   `   `   `  ���� D  @PaySystem, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:PaySystem.VIEWPaySystem.VIEW  !    PaySystem.VIEW.AdminManager   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     .localclr-namespace:PaySystem.VIEW       � Title$  后台管理程序��$	��600��$	��800�� � WindowStartupLocation$ CenterScreen=� � WindowStyle$
 None=�.���  � ColumnDefinitions �� $
��437* ��� $
��355* � � RowDefinitions �� $
��284* ��� $	��93* ��� $
��192* �.+��� -    ��Name$ textBox��$
��Left=�$��
10,82,0,10q� �� TextWrapping$
 Wrap=�$��当前钱箱钱数��$	��772��$��24(�$��2���� -    ��Name$ button1��$6��0将可找零钱箱钱币转移至不可找钱箱��$��10q�$��1���� -   $ button2��$��取出指定金额钱数��$��10,10,188,37q�$��2���� -    ��Name$ label1��$��2��$P��J请补齐钱箱钱币。5元30张；10元30张；绿/蓝灯亮起可投币��$��52��$��
10,10,10,0q�$	��Top=�$��22(�	� -   	 	�Name$	 radioButton0��$
��5.00��$
��Left=�$��17��$��
314,10,0,0q�$��2��$	��Top=�$	��113��	� -   $	 radioButton1��$��10.00��$
��Left=�$��
314,32,0,0q�$��2��$	��Top=��� -   $ button��$��关闭程序��$��1��$��10,10,21,10q�$��1��$��2��$��48(�	� -   $	 radioButton2��$��20.00��$
��Left=�$��
314,51,0,0q�$��2��$	��Top=�	� -	   $	 radioButton3��$��50.00��$
��Left=�$��20��$��
314,70,0,0q�$��2��$	��Top=�$��78��	� -
   $	 radioButton4��$��100.00��$
��Left=�$��
314,90,0,0q�$��2��$	��Top=��� -   $ label��$!��选择要取出金额钱数��$
��Left=�$��282,117,0,0q�$��2��$	��Top=�