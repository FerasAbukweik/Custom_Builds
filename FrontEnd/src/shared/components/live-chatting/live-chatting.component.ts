import {
  Component,
  DestroyRef,
  ElementRef,
  inject,
  input,
  output,
  signal,
  viewChild,
} from '@angular/core';
import { IMessageDTO } from '../../../core/DTO/message-dto';
import { LoadingComponent } from '../loading/loading.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { takeUntilDestroyed, toObservable } from '@angular/core/rxjs-interop';
import { IsVisableDirective } from '../../directives/is-visable.directive';
import { MessagesSignalRService } from '../../../core/services/client-services/messaegs-signalR-service';

@Component({
  selector: 'app-live-chatting',
  imports: [LoadingComponent, CommonModule, FormsModule, IsVisableDirective],
  templateUrl: './live-chatting.component.html',
  host: {
    class: 'w-full h-full',
  },
})
export class LiveChattingComponent {
  // DI
  private readonly _destroyRef = inject(DestroyRef);
  private readonly _messagesSignalRService = inject(MessagesSignalRService);

  // input
  messages = input.required<IMessageDTO[]>();
  isLoading = input.required<boolean>();
  quickActions = input.required<string[]>();
  isSignalRConnected = input.required<boolean>();
  isTyping = input.required<boolean>();

  // output
  lazyLoadMessages = output<void>();
  handleIsTyping = output<boolean>();
  handleSendMessage = output<string>();

  // signals
  messageInput = signal<string>('');

  // viewChild
  myScrollContainer = viewChild.required<ElementRef<HTMLDivElement>>('myScrollContainer');

  // methods

  // constructor
  constructor() {
    // manage page scroll
    let firstCheck = true;
    let secondCheck = true;

    toObservable(this.isLoading)
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe({
        next: (isLoading) => {
          if (!isLoading) {
            // if first time finished loading dont do anything (the initial value)
            if (firstCheck) firstCheck = false;
            else {
              // if second time (first time fitching messages) scroll to bottom
              if (secondCheck) {
                this._scrollToBottom();
                secondCheck = false;
              } else {
                // else stay in place
                this._stayInPlace();
              }
            }
          }
        },
      });

    // when receive a message scroll to bottom
    this._messagesSignalRService.receiveMessage$
      .pipe(takeUntilDestroyed(this._destroyRef))
      .subscribe({
        next: () => {
          this._scrollToBottom();
        },
      });
  }

  private _scrollToBottom(timeOut: number = 0) {
    setTimeout(() => {
      const div = this.myScrollContainer().nativeElement;
      div.scrollTo({
        top: div.scrollHeight,
        behavior: 'smooth',
      });
    }, timeOut);
  }

  private _stayInPlace() {
    const div = this.myScrollContainer().nativeElement;

    const oldScrollHeight = div.scrollHeight;
    const oldScrollTop = div.scrollTop;

    setTimeout(() => {
      const newScrollHeight = div.scrollHeight;
      const heightDifference = newScrollHeight - oldScrollHeight;

      div.scrollTop = oldScrollTop + heightDifference;
    }, 0);
  }

  sendMessage(message: string, isUserMessage: boolean) {
    if (isUserMessage && !this.messageInput()) return;

    this.handleSendMessage.emit(message);
    
    if (isUserMessage) this.messageInput.set('');

    this._scrollToBottom();
  }
}
