import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ResponseDisplayComponent } from './response-display.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('ResponseDisplayComponent', () => {
  let component: ResponseDisplayComponent;
  let fixture: ComponentFixture<ResponseDisplayComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [ResponseDisplayComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ResponseDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
