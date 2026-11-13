using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BulletUIController : MonoBehaviour
{
    [SerializeField] 
    private TextMeshProUGUI _bulletText;
    
    private Animation _bulletTextAnimation;
    
    [SerializeField]
    private FireHandller _fireHandller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    
    void Start()
    {
        _bulletTextAnimation = _bulletText.GetComponent<Animation>();
        _bulletText.text = _fireHandller.bulletCount.ToString("00");
       
    }

    private void OnEnable()
    {
        _fireHandller.FireEvent += OnFire;
        _fireHandller.CancelFireEvent += OnFireCancel;
    }

    private void OnDisable()
    {
        _fireHandller.FireEvent -= OnFire;
        _fireHandller.CancelFireEvent -= OnFireCancel;
    }

    private void OnFireCancel()
    {
        StopAnimation();
    }

    private void OnFire()
    {
        PlayAnimation();
        DecreaseBulletCount();
    }

    public void PlayAnimation()
    {
        _bulletTextAnimation.Play();
    }
    
    public void StopAnimation()
    {
        _bulletTextAnimation.Stop();
    }

    public void DecreaseBulletCount()
    {
        _bulletText.text = _fireHandller.bulletCount.ToString("00");
    }
    
}
