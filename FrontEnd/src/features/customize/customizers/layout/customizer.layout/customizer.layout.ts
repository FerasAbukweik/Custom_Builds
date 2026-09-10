import { Component, inject } from '@angular/core';
import { CustomizerSidebarComponent } from './components/customizer-sidebar.component/customizer-sidebar.component';
import { ActivatedRoute, RouterOutlet } from '@angular/router';
import { TopNavComponent } from '../../../../../layouts/top-nav/top-nav.component';
import { CustomBuildTypeEnum } from '../../../../../core/enums/custom-build-type-enum';
import { CustomizerSelectionService } from './customizer-selection-service';

@Component({
  selector: 'app-customizer',
  imports: [CustomizerSidebarComponent, TopNavComponent],
  providers: [CustomizerSelectionService],
  templateUrl: './customizer.layout.html',
})
export class CustomizerLayout {
  // DI
  private readonly activatedRoute = inject(ActivatedRoute);
  protected readonly customizerSelectionService = inject(CustomizerSelectionService);

  get getPageType(): CustomBuildTypeEnum {
    return this.activatedRoute.snapshot.firstChild?.data['currPage'];
  }
}
