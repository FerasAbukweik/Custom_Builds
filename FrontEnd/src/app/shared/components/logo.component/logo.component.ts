import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { projectName } from '../../../core/constants/constants';

@Component({
  selector: 'app-logo',
  imports: [RouterLink],
  templateUrl: './logo.component.html',
})
export class LogoComponent {
  goTo = input<string>();
  projectName = projectName;
}
