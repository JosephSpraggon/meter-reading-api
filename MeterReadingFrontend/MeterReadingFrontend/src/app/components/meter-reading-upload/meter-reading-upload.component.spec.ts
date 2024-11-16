import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeterReadingUploadComponent } from './meter-reading-upload.component';

describe('MeterReadingUploadComponent', () => {
  let component: MeterReadingUploadComponent;
  let fixture: ComponentFixture<MeterReadingUploadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [MeterReadingUploadComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MeterReadingUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
