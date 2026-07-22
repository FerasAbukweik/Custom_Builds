import { Component, Input } from '@angular/core';
import { IPart } from '../../../../../core/interfaces/customize-data/customize-data.model';
import { CustomizerSidebarComponent } from './components/customizer-sidebar.component/customizer-sidebar.component';
import { RouterOutlet } from '@angular/router';
import { TopNavComponent } from '../../../../../layouts/top-nav.component/top-nav.component';

@Component({
  selector: 'app-customizer',
  imports: [CustomizerSidebarComponent, TopNavComponent, RouterOutlet],
  templateUrl: './customizer.layout.html',
})
export class CustomizerLayout {
  @Input() customizeData: IPart[] = [];
}
