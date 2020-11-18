// https://github.com/vuejs/vuepress/blob/master/packages/%40vuepress/theme-default/components/Home.vue
<template>
  <div class="home">

    <div class="row">

      <div>
        <img src="/Miru-Logo-Text.png" />
      </div>

      <div class="col-md-8 order-md-1 text-center text-md-left pr-md-5">
        <h1 class="mb-3">Build full-stack .NET Core web applications with Miru</h1>
        <p class="lead mb-4">

          <Content class="theme-default-content custom" />

        </p>

        <div class="learn-more">
          <a href="/Introduction/GettingStarted.html" class="nav-link action-button">Learn More →</a>
        </div>

      </div>
    </div>

    <div class="footer">
      Copyright © 2020 <a href="http://joaofx.com" target="_blank" >Joao Carlos Clementoni</a>. 
      <small>Logo by <a href="https://logomakr.com/" target="_blank" data-logo="https://logomakr.com/2q74eV">Logomakr</a></small>
    </div>

<!-- 
    <div
      v-if="data.footer"
      class="footer"
    >
      {{ data.footer }}
    </div> -->

  </div>

</template>

<style lang="stylus">
@import '../style.styl'
@import '../override.styl'

.home
  padding 1rem 2rem 0
  max-width 960px
  margin 0px auto
  a:hover 
    text-decoration underline
  .learn-more
    text-align center
    margin 2rem
  .hero
    text-align center
    img
      max-width 100%
      max-height 280px
      display block
      margin 3rem auto 1.5rem
    h1
      font-size 3rem
    h1, .description, .action
      margin 1.8rem auto
    .description
      max-width 35rem
      font-size 1.6rem
      line-height 1.3
      color $textColor
  .action-button
    display inline-block
    font-size 1.2rem
    color #fff
    background-color $accentColor
    padding 0.8rem 1.6rem
    border-radius 4px
    transition background-color .1s ease
    box-sizing border-box
    border-bottom 1px solid darken($accentColor, 10%)
    &:hover
      background-color lighten($accentColor, 10%)
  .features
    border-top 1px solid $borderColor
    padding 1.2rem 0
    margin-top 2.5rem
    display flex
    flex-wrap wrap
    align-items flex-start
    align-content stretch
    justify-content space-between
  .feature
    flex-grow 1
    flex-basis 30%
    max-width 30%
    h2
      font-size 1.4rem
      font-weight 500
      border-bottom none
      padding-bottom 0
      color $textColor
    p
      color $textColor
  .footer
    margin 4rem
    padding 1rem
    border-top 1px solid $borderColor
    text-align center
    color $textColor

@media (max-width: $MQMobile)
  .home
    .features
      flex-direction column
    .feature
      max-width 100%
      padding 0 2.5rem

@media (max-width: $MQMobileNarrow)
  .home
    padding-left 1.5rem
    padding-right 1.5rem
    .hero
      img
        max-height 210px
        margin 2rem auto 1.2rem
      h1
        font-size 2rem
      h1, .description, .action
        margin 1.2rem auto
      .description
        font-size 1.2rem
      .action-button
        font-size 1rem
        padding 0.6rem 1.2rem
    .feature
      h2
        font-size 1.25rem
</style>
