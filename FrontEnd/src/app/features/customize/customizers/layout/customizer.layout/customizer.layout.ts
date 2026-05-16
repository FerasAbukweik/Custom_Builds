import { Component, Input } from '@angular/core';
import { IPart } from '../../../../../core/interfaces/customize-data/customize-data.model';
import { CustomizerSidebarComponent } from './components/customizer-sidebar.component/customizer-sidebar.component';
import { TopNavComponent } from '../../../../../layouts/top-nav.component/top-nav.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-customizer',
  imports: [CustomizerSidebarComponent, TopNavComponent, RouterOutlet],
  templateUrl: './customizer.layout.html',
})
export class CustomizerLayout {
  @Input() customizeData: IPart[] = [];
}
