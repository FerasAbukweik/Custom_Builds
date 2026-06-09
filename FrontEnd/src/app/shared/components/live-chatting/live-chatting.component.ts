import { Component, effect, ElementRef, input, output, signal, viewChild } from '@angular/core';
import { IMessageDTO } from '../../../core/DTO/message-dto';
import { LoadingComponent } from "../loading/loading.component";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { toObservable } from '@angular/core/rxjs-interop';
import { IsVisableDirective } from '../../directives/is-visable.directive'; 

@Component({
  selector: 'app-live-chatting',
  imports: [LoadingComponent , CommonModule , FormsModule , IsVisableDirective],
  templateUrl: './live-chatting.component.html',
  host: {
    class: 'w-full h-full'
  }
})
export class LiveChattingComponent {
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
  messageInput = signal<string>("");

  // viewChild
  myScrollContainer = viewChild.required<ElementRef<HTMLDivElement>>('myScrollContainer');



  // methods


  constructor() {
    let firstCheck = true;

    const sub = toObservable(this.isLoading).subscribe({
      next: (isLoading) => {
        if (!isLoading) {
          if(firstCheck){
            firstCheck = false;
          }
          else{
            this._scrollToBottom();
            sub.unsubscribe();
          }
        }
      },
    });


    effect(()=>{
      const currentMessages = this.messages(); // track changes in messages

      const container = this.myScrollContainer().nativeElement;

      const oldScrollHeight = container.scrollHeight;
      const oldScrollTop = container.scrollTop;

      setTimeout(() => {
        const newScrollHeight = container.scrollHeight;
        const heightDifference = newScrollHeight - oldScrollHeight;

        container.scrollTop = oldScrollTop + heightDifference;
      }, 0);
    })
  }


  // scroll to bottom
  private _scrollToBottom = (delay: number = 0) => {
    setTimeout(() => {
      this.myScrollContainer().nativeElement.scrollTo(
        0,
        this.myScrollContainer().nativeElement.scrollHeight,
      );
    }, delay);
  };


  sendMessage(message: string , emptyInput: boolean){
    if(emptyInput){
      this.messageInput.set("");
    }
    this.handleSendMessage.emit(message);

    this._scrollToBottom();
  }
}
