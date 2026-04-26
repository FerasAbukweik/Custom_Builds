import { Component, inject, input, Input } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { map } from 'rxjs';

@Component({
  selector: 'app-user-content-wrapper',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './user-content.wrapper.html',
  host: {
    class: 'w-full flex flex-col gap-8 px-8 py-10',
  },
})
export class UserContentWrapper {
  private activatedRoute = inject(ActivatedRoute);
  
  pageName = input.required<string>();
  subText = input.required<string>();
  mode = toSignal(
    this.activatedRoute.queryParams.pipe(map(params => params['mode']))
  );
}
