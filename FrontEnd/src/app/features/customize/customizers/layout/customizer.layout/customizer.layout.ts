import { Component, Input } from '@angular/core';
import { ICustomizeData } from './customizer.model';
import { CustomizerSidebarComponent } from './components/customizer-sidebar.component/customizer-sidebar.component';
import { TopNavComponent } from '../../../../../shared/components/top-nav.component/top-nav.component'; 
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-customizer',
  imports: [CustomizerSidebarComponent, TopNavComponent, RouterOutlet],
  templateUrl: './customizer.layout.html',
})
export class CustomizerLayout {
  @Input() customizeData: ICustomizeData[] = [];
}
