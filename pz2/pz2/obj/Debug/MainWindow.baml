   M S B A M L   `   `   `  ����>  :pz2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:pz2pz2  5   	   %clr-namespace:pz2.Views	pz2.Views  5   	   -clr-namespace:pz2.ViewModelpz2.ViewModel  5   	       pz2.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   #localclr-namespace:pz2     5   	   )viewsclr-namespace:pz2.Views     5   	   1	viewModelclr-namespace:pz2.ViewModel     5   	     � Title$  
MainWindow��5
   	   $	��635��5   	   $	��800��5   	    � DataContext 5      '   !pz2.ViewModel.MainWindowViewModel  5   
   6*    � 	Resources 5       �� DataType,   &pz2.ViewModel.NetworkEntitiesViewModel#   pz2.Views.NetworkEntitiesView+   %pz2.ViewModel.NetworkDisplayViewModel"   pz2.Views.NetworkDisplayView,   &pz2.ViewModel.MeasurmentGraphViewModel#   pz2.Views.MeasurmentGraphView��%�   (��       5      *, +)(�� 2     5      *, +)(�� d     5      *, +)�� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      �� 5   
   "  6   .R�5        5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6   .+�5!   
   ]� ���#635e5e$��0��5"      .+�5$      o� ���#635e5e$	��Top=�5%      $��21��5&      .@�5'      m� $
��Bold"�5(       m� CommandParameter$ NetworkEntities��5*       m� Command 5)      �� *
NavCommand+��5+      �� 5,      $	��243��6$   $��Network Entities��5-   $    �� TextAlignment$ Center=�5.   $   6=   50      m� 51      $ NetworkDisplay��52       51      �� *
NavCommand+��53      �� 54      $	��249��6$   $
��Bold"�55   $   $��Network Display��56   %   $ Center=�57   %   6>   59      m� 5:      $ MeasurementGraph��5;       5:      �� *
NavCommand+��5<      �� 5=      $	��150��5?   %   $
��Bold"�5@   $   $��Measurement Graph��5A   $   $ Center=�5B   $   6=   5D      5E      5F      � 5G   
   $��1��6   .+�5H      �� ��6   �� *CurrentViewModel+5I      5J      5K      